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
using FastTrackEServices.HelperAlgorithms;

public class ShoeRepairPut : IPut {
    public async Task<Dictionary<string, object>> put(AppDbContext appDbContext, Object idto, ITransform transform)
    {
        EditShoeRepair? dto = JsonSerializer.Deserialize<EditShoeRepair>(idto.ToString());
        Dictionary<string, object> result = new();
        ShoeRepair? repair = await appDbContext.ShoeRepairs.Where(sr => sr.Id == dto.repairId).SingleOrDefaultAsync();

        // Change client 
        int queriedFK= repair.client.Id;
        if (dto.clientID != queriedFK)
        {
            Client? newClient = await appDbContext.Clients.Where(c => c.Id == dto.clientID).SingleOrDefaultAsync();
            repair.client = newClient;
        }

        // Change Date Registered or Date Confirmed
        // Assuming Dates from are in format of MM/DD/YYYY
        string[] oldRegister = repair.dateRegistered.ToShortDateString().Split("/");
        string[] oldConfirmed = ((DateTime) repair.dateConfirmed).ToShortDateString().Split("/");

        DateTime newRegister = new DateTime(Convert.ToInt32(oldRegister[2]), Convert.ToInt32(oldRegister[0]), Convert.ToInt32(oldRegister[1]));
        DateTime newConfirmed = new DateTime(Convert.ToInt32(oldConfirmed[2]), Convert.ToInt32(oldConfirmed[0]), Convert.ToInt32(oldConfirmed[1]));

        repair.dateRegistered = newRegister;
        repair.dateConfirmed = newConfirmed;

        result["Result"] = "Success";
        return result;
    }
}