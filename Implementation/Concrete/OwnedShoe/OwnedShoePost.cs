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

public class OwnedShoePost : IPost
{
    public async Task<Dictionary<string, object>> post(AppDbContext appDbContext, Object idto)
    {
            CreateOwnedShoe dto = JsonSerializer.Deserialize<CreateOwnedShoe>(idto.ToString());
            Client? client = await appDbContext.Clients.Where(client => client.Id == dto.clientId).SingleOrDefaultAsync();
            Shoe? shoe = await appDbContext.Shoes.Where(s => s.Id == dto.shoeId).SingleOrDefaultAsync();
            Dictionary<string, object> result = new();

            if (client == null) 
            {
                result["Result"] = $"There is no corresponding Client with a client ID of {dto.clientId}";
                return result;
            } else if (shoe == null) {
                result["Result"] = $"There is no corresponding Shoe with a shoe ID of {dto.shoeId}";
                return result;
            } 

            DateTime dateNow = DateTime.Now;

            // Set Attributes
            OwnedShoe owned = new ()
            {
                client = client,
                dateAcquired = dateNow,
                shoe = shoe,
            };


            appDbContext.OwnedShoes.Add(owned);
            await appDbContext.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

}