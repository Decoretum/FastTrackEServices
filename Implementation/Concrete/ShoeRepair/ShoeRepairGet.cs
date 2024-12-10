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

public class ShoeRepairGet : IGet {

    async public Task<Dictionary<string, object>> GetAll(AppDbContext appDbContext)
    {
        object result;
        ICollection<ShoeRepair> shoeRepairs = await appDbContext.ShoeRepairs.Include("ownedShoes").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (shoeRepairs.Count == 0)
        {
            result = "There are no Shoes stored in the application";
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

    async public Task<Object> Get(AppDbContext appDbContext, int id)
    {
        ShoeRepair? shoe = await appDbContext.ShoeRepairs.Include("ownedShoes").Where(x => x.Id == id).SingleOrDefaultAsync();
        return shoe;
    }
}