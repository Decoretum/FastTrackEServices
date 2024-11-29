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

public class ClientPost : IPost
{
    public async Task<Dictionary<string, object>> post(AppDbContext appDbContext, Object idto)
    {
            CreateClient dto = JsonSerializer.Deserialize<CreateClient>(idto.ToString());
            Dictionary<string, object> result = new();

            //Date
            // For date in DTO, we format it to YYYY-MM-DD
            // Assuming that the frontend date is formatted to MM-DD-YYYY
            Console.WriteLine(dto.username.Length);
            DateTime now = DateTime.Now;
            string[] unparsedDate = dto.dateOfBirth.Split("-");
            int dtoMonth = Convert.ToInt32(unparsedDate[0]);
            int dtoDay = Convert.ToInt32(unparsedDate[1]);
            int dtoYear = Convert.ToInt32(unparsedDate[2]);
            
            int nowMonth = DateTime.Now.Month;
            int nowDay = DateTime.Now.Day;
            int nowYear = DateTime.Now.Year;
            
            if (dtoMonth > nowMonth)
            {
                nowYear -= 1;
                nowMonth += 12;
            }

            int calculatedAge = nowYear - dtoYear;
            bool checkExisting = appDbContext.Clients.Any(client => client.username == dto.username);
            bool invalidDate = (calculatedAge <= 10); 

            // Constraints
            if (dto.username.Length <= 5)
            {
                result["Result"] = "The Shoe name must be greater than 5 characters";
                return result;
            } else if (invalidDate == true)
            {
                result["Result"] = "The Client is less than or equal to 10 years old which isn't allowed";
                return result;
            } 
            else if (checkExisting)
            {
                result["Result"] = $"There is already an existing client with a name of \"{dto.username}\"";
                return result;
            }

            DateTime birth = new DateTime(dtoYear, dtoMonth, dtoDay);
            
            Client client= new ()
            {
                username = dto.username,
                dateOfBirth = birth,
                location = dto.location,
                contactNumber = dto.contactNumber
            };

            appDbContext.Clients.Add(client);
            await appDbContext.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

}