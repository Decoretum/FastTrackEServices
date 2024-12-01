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

public class ClientPut : IPut
{
    public async Task<Dictionary<string, object>> put(AppDbContext appDbContext, Object idto, ITransform transform)
    {
            //Constraints
            CollectionToStringArray transformArray = (CollectionToStringArray) transform;
            Dictionary<string, object> result = new();
            EditClient? dto = JsonSerializer.Deserialize<EditClient>(idto.ToString());
            List<Client> checkExisting = await appDbContext.Clients.Where(client => client.username == dto.username).ToListAsync();
            bool sameClient = (await appDbContext.Clients?.Where(client => client.Id == dto.Id).SingleOrDefaultAsync()).username == dto?.username;
            bool invalidNumber = dto.contactNumber.Length > 11;

            if (dto.username.Length <= 5)
            {
                result["Result"] = "The Client name must be greater than 5 characters";
                return result;
            } else if (checkExisting.Count >= 2 && sameClient == false)
            {
                result["Result"] = $"There is already an existing client with a name of {dto.username}";
                return result;
            }
            else if (invalidNumber == true)
            {
                result["Result"] = "The contact number must be less than or equal to 11 digits";
                return result;
            }
            Client? toBeEdited = await appDbContext.Clients.Where(client => client.Id == dto.Id).SingleOrDefaultAsync();

            //Date
            // For date in DTO, we format it to YYYY-MM-DD
            // Assuming that the frontend date is formatted to MM-DD-YYYY
            string[] dateArray = dto.dateOfBirth.Split("-");
            int dtoMonth = Convert.ToInt32(dateArray[0]);
            int dtoDay = Convert.ToInt32(dateArray[1]);
            int dtoYear = Convert.ToInt32(dateArray[2]);

            DateTime dateNow = DateTime.Now;
            int nowDay = dateNow.Day;
            int nowMonth = dateNow.Month;
            int nowYear = dateNow.Year;

            if (dtoMonth > nowMonth)
            {
                nowYear -= 1;
                nowMonth += 1;
            }

            int age = nowYear - dtoYear;
            bool invalidAge = age <= 10;

            if (invalidAge == true)
            {
                result["Result"] = "The client is less than or equal to 10 years old which isn't allowed";
                return result;
            }

            DateTime birthDate = new DateTime(dtoYear, dtoMonth, dtoDay);
            
            // Editing Client Properties
            appDbContext.Entry(toBeEdited).Property(client => client.username).CurrentValue = dto.username;
            appDbContext.Entry(toBeEdited).Property(client => client.dateOfBirth).CurrentValue = birthDate;
            appDbContext.Entry(toBeEdited).Property(client => client.location).CurrentValue = dto.location;
            appDbContext.Entry(toBeEdited).Property(client => client.contactNumber).CurrentValue = dto.contactNumber;
            
            await appDbContext.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

}