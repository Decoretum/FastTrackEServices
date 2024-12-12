using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FastTrackEServices.Implementation;

public class ShoeRepairRest : IRestOperation {

    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<ShoeRepair> shoeRepairs = await context.ShoeRepairs.Include("client").Include("ownedShoes").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (shoeRepairs.Count == 0)
        {
            result = "There are no Shoe repairs stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] repairArray = new object[shoeRepairs.Count];
            int j = 0;
            foreach (ShoeRepair s in shoeRepairs)
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

                foreach (OwnedShoe shoe in s.ownedShoes)
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
        ShoeRepair? shoe = await context.ShoeRepairs.Include("ownedShoes").Where(x => x.Id == id).SingleOrDefaultAsync();
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
            ShoeRepair repair = new ()
            {
                client = client,
                dateConfirmed = null,
                dateRegistered = dateNow
            };

            // Set ownedShoes relationship
            for (int i = 0; i <= dto.ownedShoes.Length - 1; i++)
            {
                int ownedShoeId = dto.ownedShoes[i];
                OwnedShoe? owned = await context.OwnedShoes.Where(os => os.Id == ownedShoeId && os.client == client).SingleOrDefaultAsync();
                if (owned == null)
                {
                    result["Result"] = $"Client {client.username} has no owned shoe of ID {ownedShoeId}";
                    return result;
                } else {
                    owned.shoeRepair = repair;
                }
            }

            context.ShoeRepairs.Add(repair);
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        EditShoeRepair? dto = JsonSerializer.Deserialize<EditShoeRepair>(idto.ToString());
        Dictionary<string, object> result = new();
        ShoeRepair? repair = context.ShoeRepairs.Include("client").Include("ownedShoes").Where(sr => sr.Id == dto.repairId).SingleOrDefault();

        // Change client 
        Client queriedClient = context.Clients.Include("shoeRepairs").Where(c => c.Id == dto.clientId).SingleOrDefault();
        // Console.WriteLine("DTO CLIENT: " + dto.clientId);
        // Console.WriteLine("Repair ID: " + dto.repairId);
        // Console.WriteLine(queriedClient == null); // THIS IS THE 
        
        // Change client and ownedshoe
        if (repair.client != queriedClient)
        {
            Client? newClient = queriedClient;
            foreach (OwnedShoe owned in repair.ownedShoes)
            {
                owned.shoeRepair = null;    
            }
            repair.client = newClient;

            // Repoint owned shoes relationship to the shoe repair
            // DTO array contains pk of owned shoes of client
            for (int i = 0; i <= dto.ownedShoesArray.Length - 1; i++)
            {
                int arrayId = dto.ownedShoesArray[i];
                OwnedShoe owned = context.OwnedShoes.Where(os => os.Id ==arrayId).SingleOrDefault();
                owned.shoeRepair = repair;
            }

            // Remove owned shoes that were dislodged
            ICollection<OwnedShoe> myShoes = repair.ownedShoes;
            foreach (OwnedShoe shoe in myShoes)
            {
                int pk = shoe.Id;
                if (!dto.ownedShoesArray.Contains(pk))
                {
                    shoe.shoeRepair = null;
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
        ShoeRepair toBeDeleted = context.ShoeRepairs.Include("ownedShoes").Where(sr => sr.Id == id).SingleOrDefault();
        ICollection<OwnedShoe> ownedShoes = toBeDeleted.ownedShoes;

        foreach (OwnedShoe owned in ownedShoes)
        {
            owned.shoeRepair = null;
        }

        context.ShoeRepairs.Remove(toBeDeleted);
        await context.SaveChangesAsync();
    }
}