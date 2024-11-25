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

public class ShoeGet : IGet {

    async public Task<Dictionary<string, object>> GetAll(AppDbContext appDbContext)
    {
        object result;
        ICollection<Shoe> shoes = await appDbContext.Shoes.Include("shoeColors").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (shoes.Count == 0)
        {
            result = "There are no Shoes stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] shoeArray = new object[shoes.Count];
            int j = 0;
            foreach (Shoe s in shoes)
            {
                CreateShoe shoe = new()
                {
                    name = s.name,
                    brand = s.brand,
                    description = s.description
                };
                string[] colors = new string[s.shoeColors.Count];
                int i = 0;
                foreach (ShoeColor color in s.shoeColors)
                {
                    colors[i] = color.name;
                    i++;
                }
                shoe.shoeColors = colors;
                shoeArray[j] = shoe;
                j++;
            }
            result = shoeArray;
            keyValue["Result"] = result;
        }
        // await appDbContext.DisposeAsync();
        return keyValue; 
    }

    async public Task<Object> Get(AppDbContext appDbContext, int id)
    {
        Shoe? shoe = await appDbContext.Shoes.Include("shoeColors").Where(x => x.Id == id).SingleOrDefaultAsync();
        // await appDbContext.DisposeAsync();
        return shoe;
    }
}