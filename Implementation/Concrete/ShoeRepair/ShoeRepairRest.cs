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
        ICollection<ShoeRepair> shoeRepairs = await context.ShoeRepairs.Include("ownedShoes").ToListAsync();
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
                string confirmedDate = ((DateTime) s.dateConfirmed).ToShortDateString();
                GetShoeRepair repair = new()
                {
                    client= s.client,
                    dateRegistered = registerDate,
                    dateConfirmed = confirmedDate
                };
                string[] owned = new string[s.ownedShoes.Count];
                int i = 0;
                foreach (OwnedShoe shoe in s.ownedShoes)
                {
                    owned[i] = shoe.shoe.name;
                    i++;
                }
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
            Client? client = await context.Clients.Where(client => client.Id == dto.clientID).SingleOrDefaultAsync();
            Dictionary<string, object> result = new();

            if (client == null) 
            {
                result["Result"] = $"There is no corresponding client with a client ID of {dto.clientID}";
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
                string shoeName = dto.ownedShoes[i];
                OwnedShoe? owned = await context.OwnedShoes.Where(os => os.shoe.name == shoeName).SingleOrDefaultAsync();
                if (owned == null)
                {
                    result["Result"] = $"You are trying to create a shoe repair for an unknown shoe named {shoeName}";
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
        ShoeRepair? repair = await context.ShoeRepairs.Where(sr => sr.Id == dto.repairId).SingleOrDefaultAsync();

        // Change client 
        int queriedFK= repair.client.Id;
        if (dto.clientID != queriedFK)
        {
            Client? newClient = await context.Clients.Where(c => c.Id == dto.clientID).SingleOrDefaultAsync();
            repair.client = newClient;
        }

        // Change Date Registered or Date Confirmed
        // Assuming Dates from are in format of MM/DD/YYYY
        string[] oldRegister = repair.dateRegistered.ToShortDateString().Split("/");
        string[] oldConfirmed = ((DateTime) repair.dateConfirmed).ToShortDateString().Split("/");

        DateTime newRegister = new DateTime(Convert.ToInt32(oldRegister[2]), Convert.ToInt32(oldRegister[0]), Convert.ToInt32(oldRegister[1]));
        DateTime newConfirmed = new DateTime(Convert.ToInt32(oldConfirmed[2]), Convert.ToInt32(oldConfirmed[0]), Convert.ToInt32(oldConfirmed[1]));

        repair.dateRegistered = newRegister;
        repair.dateConfirmed = newConfirmed;

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