using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FastTrackEServices.Implementation;

public class ShoeOrderRest : IRestOperation {

    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<OwnedShoe> ownedShoes = await context.OwnedShoes.Include("shoe").Include("client").Include("shoeRepair").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (ownedShoes.Count == 0)
        {
            result = "There are no Owned Shoes stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] ownedArray = new object[ownedShoes.Count];
            int j = 0;
            foreach (OwnedShoe os in ownedShoes)
            {
                // default is english culture date representation
                // MM/DD/YYYY
                string acquiredDate = os.dateAcquired.ToShortDateString(); 

                // Get Client
                Task<Client> getClient = Task.Run(() => {
                    return os.client;
                });

                // Get Shoe
                Task<Shoe> getShoe = Task.Run(async () => {
                    Shoe queriedShoe = await context.Shoes.Include("shoeColors").Where(shoe => shoe.Id == os.shoe.Id).SingleOrDefaultAsync();
                    return queriedShoe;
                });

                GetShoe shoeDto = new GetShoe()
                {
                    id = getShoe.Result.Id,
                    name =  getShoe.Result.name,
                    brand = getShoe.Result.brand,
                    description = getShoe.Result.description
                };

                int i = 0;
                string[] colorArray = new string[getShoe.Result.shoeColors.Count];
                foreach (ShoeColor color in getShoe.Result.shoeColors)
                {
                    colorArray[i] = color.name;
                    i++;
                }

                shoeDto.shoeColors = colorArray;

                GetOwnedShoe ownedDto = new GetOwnedShoe()
                {
                    id = os.Id,
                    client = await getClient,
                    shoe = shoeDto,
                    dateAcquired = acquiredDate
                };

                if (os.shoeRepair == null) // negative ID means not yet assigned
                ownedDto.shoeRepairId = -1;

                else
                ownedDto.shoeRepairId = os.shoeRepair.Id;

                ownedArray[j] = ownedDto;
                j++;
            }
            result = ownedArray;
            keyValue["Result"] = result;
        }
        return keyValue;
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        OwnedShoe? ownedShoe = await context.OwnedShoes.Include("shoe").Include("client").Include("shoeRepair").Where(x => x.Id == id).SingleOrDefaultAsync();
        
        Task<Shoe> getShoe = Task.Run(async () => {
            Shoe queriedShoe = await context.Shoes.Include("shoeColors").Where(shoe => shoe.Id == ownedShoe.shoe.Id).SingleOrDefaultAsync();
            return queriedShoe;
        });

        GetShoe shoeDto = new GetShoe()
        {
            id = getShoe.Result.Id,
            name =  getShoe.Result.name,
            brand = getShoe.Result.brand,
            description = getShoe.Result.description
        };

        int i = 0;
        string[] colorArray = new string[getShoe.Result.shoeColors.Count];
        foreach (ShoeColor color in getShoe.Result.shoeColors)
        {
            colorArray[i] = color.name;
            i++;
        }

        shoeDto.shoeColors = colorArray;

        GetOwnedShoe ownedDto = new GetOwnedShoe()
        {
            id = ownedShoe.Id,  
            client = ownedShoe.client,
            shoe = shoeDto,
            dateAcquired = ownedShoe.dateAcquired.ToShortDateString()
        };


        if (ownedShoe.shoeRepair == null)
        ownedDto.shoeRepairId = -1;

        else
        ownedDto.shoeRepairId = ownedShoe.shoeRepair.Id;
        
        return ownedDto;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
    {
        Dictionary<string, object> result = new();
        
        CreateOwnedShoe dto = JsonSerializer.Deserialize<CreateOwnedShoe>(idto.ToString());
        Client client =  context.Clients.Where(client => client.Id == dto.clientId).SingleOrDefault();
        Shoe shoe = context.Shoes.Where(s => s.Id == dto.shoeId).SingleOrDefault();
        
        if (client == null) 
        {
            result["Result"] = $"There is no corresponding Client with a client ID of {dto.clientId}";
            return result;
        }  
        
        if (shoe == null) {
            result["Result"] = $"There is no corresponding Shoe with a shoe ID of {dto.shoeId}";
            return result;
        } 

        DateTime dateNow = DateTime.Now;

        OwnedShoe owned = new ()
        {
            client = client,
            dateAcquired = dateNow,
            shoe = shoe,
            shoeRepair = null
        };


        context.OwnedShoes.Add(owned);
        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
        
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        Dictionary<string, Object> result = new();
        EditOwnedShoe dto = JsonSerializer.Deserialize<EditOwnedShoe>(idto.ToString());
        OwnedShoe ownedShoe = await context.OwnedShoes.Include("shoeRepair").Include("shoe").Include("client").Where(os => os.Id == dto.ownedShoeId).SingleOrDefaultAsync();

        // Change client
        if (dto.clientId != ownedShoe.client.Id)
        {
            Client newClient = await context.Clients.Where(c => c.Id == dto.clientId).SingleOrDefaultAsync();
            ownedShoe.client = newClient;
        }

        // Change Shoe
        if (dto.shoeId != ownedShoe.shoe.Id)
        {
            Shoe newShoe = await context.Shoes.Where(s => s.Id == dto.shoeId).SingleOrDefaultAsync();
            ownedShoe.shoe = newShoe;
        }

        // Change Shoe Repair
        // By default, shoeRepairId is null or -1 if no shoe repair is being operated
        if (dto.shoeRepairId != null || dto.shoeRepairId != ownedShoe.shoeRepair.Id || dto.shoeRepairId != -1)
        {
            ShoeRepair shoeRepair = await context.ShoeRepairs.Where(repair => repair.Id == dto.shoeRepairId).SingleOrDefaultAsync();
            ownedShoe.shoeRepair = shoeRepair;
        } if (dto.shoeRepairId == -1 || dto.shoeRepairId == null)
        {
            ownedShoe.shoeRepair = null;
        }

        // Assuming date from frontend is formatted to: MM/DD/YYYY
        string[] dateArray = dto.dateAcquired.Split("/");
        DateTime newDate = new DateTime(Convert.ToInt32(dateArray[2]), Convert.ToInt32(dateArray[0]), Convert.ToInt32(dateArray[1]));
        DateTime now = DateTime.Now;
        if (newDate > now)
        {
            result["Result"] = "Date Acquired for the owned shoe cannot be later than the current time";
            return result;
        }
        
        ownedShoe.dateAcquired = newDate;

        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        OwnedShoe toBeDeleted = await context.OwnedShoes.Where(os => os.Id == id).SingleOrDefaultAsync();
        context.OwnedShoes.Remove(toBeDeleted);
        await context.SaveChangesAsync();
    }
}