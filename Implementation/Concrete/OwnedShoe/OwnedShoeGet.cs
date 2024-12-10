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

public class OwnedShoeGet : IGet {

    async public Task<Dictionary<string, object>> GetAll(AppDbContext appDbContext)
    {
        object result;
        ICollection<OwnedShoe> ownedShoes = await appDbContext.OwnedShoes.ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (ownedShoes.Count == 0)
        {
            result = "There are no Shoes stored in the application";
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
                GetOwnedShoe ownedDto = new()
                {
                    client = os.client,
                    shoe = os.shoe,
                    shoeRepairId = os.shoeRepair.Id,
                    dateAcquired = acquiredDate
                };

                ownedArray[j] = ownedDto;
            }
            result = ownedArray;
            keyValue["Result"] = result;
        }
        return keyValue; 
    }

    async public Task<Object> Get(AppDbContext appDbContext, int id)
    {
        OwnedShoe? shoe = await appDbContext.OwnedShoes.Where(x => x.Id == id).SingleOrDefaultAsync();
        return shoe;
    }
}