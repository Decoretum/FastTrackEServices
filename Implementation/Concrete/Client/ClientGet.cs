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

public class ClientGet : IGet {

    async public Task<Dictionary<string, object>> GetAll(AppDbContext appDbContext)
    {

        object result;
        ICollection<Client> clients = await appDbContext.Clients.Include("ownedShoes").ToListAsync();
        Dictionary<string, object> keyValue = new ();
        Console.WriteLine("COUNT: " + clients.Count);
        if (clients.Count == 0)
        {
            result = "There are no Clients stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] clientArray = new object[clients.Count];
            int j = 0;
            foreach (Client s in clients)
            {
                GetClient client = new()
                {
                    username = s.username,
                    dateOfBirth = s.dateOfBirth.ToShortDateString(),
                    contactNumber = s.contactNumber,
                    location = s.location
                };
                string[] ownedShoes = new string[s.ownedShoes.Count];
                int i = 0;
                foreach (OwnedShoe ownedShoe in s.ownedShoes)
                {
                    ownedShoes[i] = ownedShoe.shoe.name;
                    i++;
                }
                client.ownedShoes = ownedShoes;
                clientArray[j] = client;
                j++;
            }
            result = clientArray;
            keyValue["Result"] = result;
        }
        return keyValue; 
    }

    async public Task<Object> Get(AppDbContext appDbContext, int id)
    {
        Client? client = await appDbContext.Clients.Include("ownedShoes").Where(x => x.Id == id).SingleOrDefaultAsync();
        return client;
    }
}