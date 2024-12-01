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

public class ClientDelete : IDelete
{
    public async Task delete(AppDbContext appDbContext, int id)
    {
        Client toBeDeleted = await appDbContext.Clients.Include("ownedShoes").Include("shoeRepairs").Where(client => client.Id == id).SingleOrDefaultAsync();
        ICollection<OwnedShoe> owned = toBeDeleted.ownedShoes;
        ICollection<ShoeRepair> repairs = toBeDeleted.shoeRepairs;

        // Delete Shoe Repairs
        appDbContext.ShoeRepairs.RemoveRange(repairs);

        // Delete Shoe Owned
        appDbContext.OwnedShoes.RemoveRange(owned);

        // Delete Shoe
        appDbContext.Clients.Remove(toBeDeleted);

        // Delete OwnedShoe
        await appDbContext.SaveChangesAsync();

    }

}