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

public class ShoeRepairDelete : IDelete
{
    public async Task delete(AppDbContext appDbContext, int id)
    {
        ShoeRepair toBeDeleted = await appDbContext.ShoeRepairs.Where(sr => sr.Id == id).SingleOrDefaultAsync();

        appDbContext.ShoeRepairs.Remove(toBeDeleted);

        // Delete OwnedShoe
        await appDbContext.SaveChangesAsync();

    }

}