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

public class ShoeDelete : IDelete
{
    public async Task delete(AppDbContext appDbContext, int id)
    {
        Shoe toBeDeleted = await appDbContext.Shoes.Include("shoeColors").Where(shoe => shoe.Id == id).SingleOrDefaultAsync();
        ICollection<ShoeColor> colors = toBeDeleted.shoeColors;

        // Delete ShoeColors
        appDbContext.ShoeColors.RemoveRange(colors);

        // Delete Shoe
        appDbContext.Shoes.Remove(toBeDeleted);

        // Delete OwnedShoe

        await appDbContext.SaveChangesAsync();

    }

}