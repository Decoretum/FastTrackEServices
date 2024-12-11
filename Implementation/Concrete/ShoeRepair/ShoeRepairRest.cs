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
                string registerDate = s.dateRegistered.ToShortDateString(); 
                string confirmedDate;

                if (s.dateConfirmed != null)
                confirmedDate = ((DateTime) s.dateConfirmed).ToShortDateString();

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
        Console.WriteLine("DTO CLIENT: " + dto.clientId);
        Console.WriteLine("Repair ID: " + dto.repairId);
        Console.WriteLine(queriedClient == null); // THIS IS THE CULPRIT
        // if (repair.client != queriedClient)
        // {
        //     Client? newClient = queriedClient;
        //     foreach (OwnedShoe owned in repair.ownedShoes)
        //     {
        //         owned.shoeRepair = null;    
        //     }
        //     repair.client = newClient;
        //     context.ShoeRepairs.Remove(repair);
        // }

        // Change Date Registered or Date Confirmed
        // Assuming Dates from are in format of MM/DD/YYYY
        string[] oldRegister = repair.dateRegistered.ToShortDateString().Split("/");
        DateTime newConfirmed = DateTime.MaxValue;
        string[] oldConfirmed;

        if (dto.dateConfirmed != null)
        newConfirmed = dto.dateConfirmed


        if (repair.dateConfirmed != null)
        {
            Console.WriteLine(((DateTime)repair.dateConfirmed).ToShortDateString());
            oldConfirmed = ((DateTime) repair.dateConfirmed).ToShortDateString().Split("/");
            newConfirmed = new DateTime(Convert.ToInt32(oldConfirmed[2]), Convert.ToInt32(oldConfirmed[1]), Convert.ToInt32(oldConfirmed[0]));
        }
        
        else
        {
            oldConfirmed = null;
        }
        
        
        DateTime newRegister = new DateTime(Convert.ToInt32(oldRegister[2]), Convert.ToInt32(oldRegister[1]), Convert.ToInt32(oldRegister[0]));

        if (newRegister > newConfirmed)
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
        ShoeRepair toBeDeleted = await context.ShoeRepairs.Where(sr => sr.Id == id).SingleOrDefaultAsync();

        context.ShoeRepairs.Remove(toBeDeleted);

        // Delete OwnedShoe
        await context.SaveChangesAsync();
    }
}