using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FastTrackEServices.Implementation;

public class ShoewareRepairRest : IRestOperation {

    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<ShoewareRepair> shoeRepairs = await context.ShoewareRepairs.Include("client").Include("ownedShoes").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (shoeRepairs.Count == 0)
        {
            result = "There are no Shoe repairs stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] repairArray = new object[shoeRepairs.Count];
            int j = 0;
            foreach (ShoewareRepair s in shoeRepairs)
            {
                // default is english culture date representation
                // MM/DD/YYYY
                string registerDate = s.dateRegistered.ToString("MM-dd-yyyy"); 
                string confirmedDate;

                if (s.dateConfirmed != null)
                confirmedDate = ((DateTime) s.dateConfirmed).ToString("MM-dd-yyyy");

                else
                confirmedDate = null;

                GetShoeRepair repair = new()
                {
                    Id = s.Id,
                    client= s.client,
                    dateRegistered = registerDate,
                    dateConfirmed = confirmedDate
                };

                int[] owned = new int[s.ownedShoes.Count];
                int i = 0;

                foreach (OwnedShoeware shoe in s.ownedShoes)
                {
                    owned[i] = shoe.Id;
                    i++;
                }

                repair.ownedShoes = owned;

                repairArray[j] = repair;
                j++;
            }
            result = repairArray;
            keyValue["Result"] = result;
        }
        return keyValue;
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        ShoewareRepair? shoe = await context.ShoewareRepairs.Include("ownedShoes").Where(x => x.Id == id).SingleOrDefaultAsync();
        return shoe;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
    {
        CreateShoeRepair dto = JsonSerializer.Deserialize<CreateShoeRepair>(idto.ToString());
            Client? client = await context.Clients.Where(client => client.Id == dto.clientId).SingleOrDefaultAsync();
            Dictionary<string, object> result = new();

            if (client == null) 
            {
                result["Result"] = $"There is no corresponding client with a client ID of {dto.clientId}";
                return result;
            }

            DateTime dateNow = DateTime.Now;

            // Set Attributes
            ShoewareRepair repair = new ()
            {
                client = client,
                dateConfirmed = null,
                dateRegistered = dateNow
            };

            // Set ownedShoes relationship
            for (int i = 0; i <= dto.ownedShoes.Length - 1; i++)
            {
                int ownedShoeId = dto.ownedShoes[i];
                OwnedShoeware? owned = await context.OwnedShoewares.Where(os => os.Id == ownedShoeId && os.client == client).SingleOrDefaultAsync();
                if (owned == null)
                {
                    result["Result"] = $"Client {client.username} has no owned shoe of ID {ownedShoeId}";
                    return result;
                } else {
                    owned.shoewareRepair = repair;
                }
            }

            context.ShoewareRepairs.Add(repair);
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        EditShoeRepair? dto = JsonSerializer.Deserialize<EditShoeRepair>(idto.ToString());
        Dictionary<string, object> result = new();
        ShoewareRepair? repair = context.ShoewareRepairs.Include("client").Include("ownedShoes").Where(sr => sr.Id == dto.repairId).SingleOrDefault();

        if (dto.confirming == "True")
        {
            context.Entry(repair).Property(r => r.dateConfirmed).CurrentValue = DateTime.Now;
            result["Result"] = "Success";
            context.SaveChangesAsync();
            return result;
        }

        // Change client 
        Client queriedClient = context.Clients.Include("shoeRepairs").Where(c => c.Id == dto.clientId).SingleOrDefault();

        // Change client and ownedshoe
        if (repair.client != queriedClient)
        {
            Client? newClient = queriedClient;
            foreach (OwnedShoeware owned in repair.ownedShoes)
            {
                owned.shoewareRepair = null;    
            }
             
            repair.client = newClient;

        }

        // Repoint owned shoes relationship to the shoe repair
        // DTO array contains pk of owned shoes of client
        if (dto.ownedShoesArray.Length > 0)
        {
            for (int i = 0; i <= dto.ownedShoesArray.Length - 1; i++)
            {
                int arrayId = dto.ownedShoesArray[i];
                OwnedShoeware owned = context.OwnedShoewares.Where(os => os.Id ==arrayId).SingleOrDefault();
                owned.shoewareRepair = repair;
            }

            if (repair.client == queriedClient)
            {
                // Remove owned shoes that were dislodged
                ICollection<OwnedShoeware> myShoes = repair.ownedShoes;
                foreach (OwnedShoeware shoe in myShoes)
                {
                    int pk = shoe.Id;
                    if (!dto.ownedShoesArray.Contains(pk))
                    {
                        shoe.shoewareRepair = null;
                    }
                }          
            } 

             
        }
        

        // Change Date Registered or Date Confirmed
        // Assuming Dates from are in format of MM/DD/YYYY
        string[] oldRegister = repair.dateRegistered.ToShortDateString().Split("/");
        string[] newRegisterArray = dto.dateRegistered.Split("/");
        DateTime? newConfirmed = ((DateTime?) repair.dateConfirmed);

        // User wants to change date confirmed or not
        if (dto.dateConfirmed != null)
        {
            string[] newConfirmedArray = dto.dateConfirmed.Split("/");
            newConfirmed = new DateTime(Convert.ToInt32(newConfirmedArray[2]), Convert.ToInt32(newConfirmedArray[0]), Convert.ToInt32(newConfirmedArray[1]));
        }
        
        DateTime newRegister = new DateTime(Convert.ToInt32(newRegisterArray[2]), Convert.ToInt32(newRegisterArray[0]), Convert.ToInt32(newRegisterArray[1]));

        if (newConfirmed != null && newRegister > newConfirmed)
        {
            result["Result"] = "Register Date cannot be later than Confirmed Date";
            return result;
        }

        repair.dateRegistered = newRegister;
        repair.dateConfirmed = newConfirmed;

        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        ShoewareRepair toBeDeleted = context.ShoewareRepairs.Include("ownedShoes").Where(sr => sr.Id == id).SingleOrDefault();
        ICollection<OwnedShoeware> ownedShoes = toBeDeleted.ownedShoes;

        foreach (OwnedShoeware owned in ownedShoes)
        {
            owned.shoewareRepair = null;
        }

        context.ShoewareRepairs.Remove(toBeDeleted);
        await context.SaveChangesAsync();
    }
}