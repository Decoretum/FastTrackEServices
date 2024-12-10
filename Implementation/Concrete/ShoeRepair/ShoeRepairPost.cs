namespace Implementation.Concrete;
using System.Text.Json;
using System.Text;
using System.Web;
using MySql.Data.MySqlClient;

using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using FastTrackEServices.Implementation;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

public class ShoeRepairPost : IPost
{
    public async Task<Dictionary<string, object>> post(AppDbContext appDbContext, Object idto)
    {
            CreateShoeRepair dto = JsonSerializer.Deserialize<CreateShoeRepair>(idto.ToString());
            Client? client = await appDbContext.Clients.Where(client => client.Id == dto.clientID).SingleOrDefaultAsync();
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
                OwnedShoe? owned = await appDbContext.OwnedShoes.Where(os => os.shoe.name == shoeName).SingleOrDefaultAsync();
                if (owned == null)
                {
                    result["Result"] = $"You are trying to create a shoe repair for an unknown shoe named {shoeName}";
                    return result;
                } else {
                    owned.shoeRepair = repair;
                }
            }

            appDbContext.ShoeRepairs.Add(repair);
            await appDbContext.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

}