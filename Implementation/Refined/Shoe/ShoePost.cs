namespace Implementation.Refined;
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

public class ShoePost : IPost
{
    async public void post(AppDbContext appDbContext, CreateShoe dto)
    {
            Shoe shoe = new ()
            {
                name = dto.name,
                brand = dto.brand,
                description = dto.description
            };

            ShoeColor[] shoeColors = new ShoeColor[dto.shoeColors.Length];
            for (int i = 0; i <= dto.shoeColors.Length - 1; i++)
            {
                string dtoColorName = dto.shoeColors[i];
                ShoeColor color = new()
                {
                    name = dtoColorName,
                    shoe = shoe
                };
                shoeColors[i] = color;
            }
            appDbContext.ShoeColors.AddRange(shoeColors);
            appDbContext.Shoes.Add(shoe);
            await appDbContext.SaveChangesAsync();
            
    }

}