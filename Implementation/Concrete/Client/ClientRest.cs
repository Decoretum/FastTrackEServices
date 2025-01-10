using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FastTrackEServices.Implementation;

public class ClientRest : IRestOperation {
    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<Client> clients = await context.Clients.Include("ownedShoes").ToListAsync();
        Dictionary<string, object> keyValue = new ();
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
                    id = s.Id,
                    username = s.username,
                    dateOfBirth = s.dateOfBirth.ToShortDateString(),
                    contactNumber = s.contactNumber,
                    location = s.location
                };

                if (s.ownedShoes != null || s.ownedShoes.Count != 0)
                {
                    string[] ownedShoes = new string[s.ownedShoes.Count];
                    int i = 0;
                    foreach (OwnedShoeware os in s.ownedShoes)
                    {
                        OwnedShoeware owned = await context.OwnedShoewares.Include("shoe").Where(owned => owned.Id == os.Id).SingleAsync();
                        ownedShoes[i] = owned.shoe.name;
                        i++;
                    }
                    client.ownedShoes = ownedShoes;
                } else {
                    client.ownedShoes = null;
                }

                clientArray[j] = client;
                j++;
            }

            result = clientArray;
            keyValue["Result"] = result;
        }
        return keyValue; 
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        Client? client = await context.Clients.Include("ownedShoes").Where(x => x.Id == id).SingleOrDefaultAsync();
        return client;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
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
            bool checkExisting = context.Clients.Any(client => client.username == dto.username);
            bool invalidDate = calculatedAge <= 10; 
            bool invalidNumber = dto.contactNumber.Length > 11;

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
            else if (invalidNumber == true)
            {
                result["Result"] = "The contact number must be less than or equal to 11 digits";
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

            context.Clients.Add(client);
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        //Constraints
            CollectionToStringArray transformArray = (CollectionToStringArray) transform;
            Dictionary<string, object> result = new();
            EditClient? dto = JsonSerializer.Deserialize<EditClient>(idto.ToString());
            List<Client> checkExisting = await context.Clients.Where(client => client.username == dto.username).ToListAsync();
            bool sameClient = (await context.Clients?.Where(client => client.Id == dto.Id).SingleOrDefaultAsync()).username == dto?.username;
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
            Client? toBeEdited = await context.Clients.Where(client => client.Id == dto.Id).SingleOrDefaultAsync();

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
            context.Entry(toBeEdited).Property(client => client.username).CurrentValue = dto.username;
            context.Entry(toBeEdited).Property(client => client.dateOfBirth).CurrentValue = birthDate;
            context.Entry(toBeEdited).Property(client => client.location).CurrentValue = dto.location;
            context.Entry(toBeEdited).Property(client => client.contactNumber).CurrentValue = dto.contactNumber;
            
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        Client toBeDeleted = await context.Clients.Include("ownedShoes").Include("orderCarts").Include("shoeRepairs").Where(client => client.Id == id).SingleOrDefaultAsync();
        ICollection<OwnedShoeware> owned = toBeDeleted.ownedShoes;
        ICollection<ShoewareRepair> repairs = toBeDeleted.shoeRepairs;
        ICollection<OrderCart> orders = toBeDeleted.orderCarts;

        // Delete Order Carts
        context.OrderCarts.RemoveRange(orders);

        // Delete Shoe Repairs
        context.ShoewareRepairs.RemoveRange(repairs);

        // Delete Shoe Owned
        context.OwnedShoewares.RemoveRange(owned);

        // Delete Client
        context.Clients.Remove(toBeDeleted);

        // Delete OwnedShoe
        await context.SaveChangesAsync();
    }
}