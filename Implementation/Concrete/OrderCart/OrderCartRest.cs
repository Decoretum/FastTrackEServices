using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FastTrackEServices.Implementation;

public class OrderCartRest : IRestOperation {
    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<OrderCart> carts = await context.OrderCarts.Include("shoeOrders").Include("client").ToListAsync();
        Dictionary<string, object> keyValue = new ();
        if (carts.Count == 0)
        {
            result = "There are no Order Carts stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] cartArray = new object[carts.Count];
            int j = 0;
            foreach (OrderCart oc in carts)
            {   
                string newConfirmed;

                if (oc.dateConfirmed == null)
                newConfirmed = "Not Yet Confirmed";

                else
                newConfirmed = ((DateTime) oc.dateConfirmed).ToShortDateString();
                
                String newRegistered = oc.dateRegistered.ToShortDateString();

                GetOrderCart cart = new()
                {
                    Id = oc.Id,
                    clientName = oc.client.username,
                    cart_name = oc.cart_name,
                    dateRegistered = newRegistered,
                    dateConfirmed = newConfirmed
                };

                if (oc.shoeOrders != null || oc.shoeOrders.Count != 0)
                {
                    int[] shoeOrders = new int[oc.shoeOrders.Count];
                    int i = 0;
                    foreach (ShoeOrder o in oc.shoeOrders)
                    {
                        shoeOrders[i] = o.Id;
                        i++;
                    }
                    cart.shoeOrders = shoeOrders;
                } else {
                    cart.shoeOrders = null;
                }

                cartArray[j] = cart;
                j++;
            }

            result = cartArray;
            keyValue["Result"] = result;
        }
        return keyValue; 
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        OrderCart? cart = await context.OrderCarts.Include("client").Include("shoeOrders").Where(x => x.Id == id).SingleOrDefaultAsync();
        
        Dictionary<string, object> result = new Dictionary<string, object>();
        if (cart == null)
        {
            result["Result"] = $"There is no Order Cart with an ID of {id}";
            return result;
        }

        GetOrderCart dto = new GetOrderCart()
        {
            Id = cart.Id,
            clientName = cart.client.username,
            cart_name = cart.cart_name,
            dateRegistered = cart.dateRegistered.ToShortDateString(),
        };

        if (cart.dateConfirmed == null)
        dto.dateConfirmed = "Not Yet Confirmed";

        else
        dto.dateConfirmed = ((DateTime) cart.dateConfirmed).ToShortDateString();

        ICollection<ShoeOrder> orders = cart.shoeOrders;
        int[] shoeOrders = new int[orders.Count];
        int i = 0;
        foreach (ShoeOrder order in orders)
        {
            shoeOrders[i] = order.Id;
            i++;
        }

        dto.shoeOrders = shoeOrders;
        return dto;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
    {
        CreateOrderCart dto = JsonSerializer.Deserialize<CreateOrderCart>(idto.ToString());
        Dictionary<string, object> result = new();

        //Date
        // For date in DTO, we format it to YYYY-MM-DD
        // Assuming that the frontend date is formatted to MM-DD-YYYY
        DateTime now = DateTime.Now;
        string[] unparsedDate = dto.dateRegistered.Split("-");
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

        // Find Client
        Client queriedClient = context.Clients.Where(c => c.Id == dto.clientId).Include("orderCarts").SingleOrDefault();
        bool clientTrue = queriedClient != null;

        // Constraints
        if (clientTrue != true)
        {
            result["Result"] = $"Client with an ID of ${dto.clientId} does not exist in the system";
            return result;
        }
        

        // Name of Cart
        ICollection<OrderCart> carts = queriedClient.orderCarts;
        int count = carts.Count + 1;

        string suffix = count % 2 == 0 ? "nd" : "st";
        string cartName = $"{queriedClient.username}'s {count}{suffix} Order Cart";

        DateTime registered = new DateTime(dtoYear, dtoMonth, dtoDay);

        OrderCart orderCart = new()
        {
            dateRegistered = now,
            dateConfirmed = null,
            client = queriedClient,
            cart_name = cartName
        };

        context.OrderCarts.Add(orderCart);
        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        //Constraints
        CollectionToStringArray transformArray = (CollectionToStringArray) transform;
        Dictionary<string, object> result = new();
        EditOrderCart? dto = JsonSerializer.Deserialize<EditOrderCart>(idto.ToString());
        List<Client> checkExisting = await context.Clients.Where(client => client.username == dto.clientUsername).ToListAsync();

        OrderCart cart = context.OrderCarts.Where(oc => oc.Id == dto.Id).Include("client").SingleOrDefault();
        
        if (dto.confirming == "True")
        {
            context.Entry(cart).Property(cart => cart.dateConfirmed).CurrentValue = DateTime.Now;
            result["Result"] = "Success";
            await context.SaveChangesAsync();
            return result;
        }

        // Editing Client and cart name
        Client queriedClient = context.Clients.Where(c => c.username == dto.clientUsername).Include("orderCarts").SingleOrDefault();
        Client currentClient = cart.client;
        if (queriedClient == null)
        {
            result["Result"] = $"There is no client with a name of {dto.clientUsername}";
            return result;
        }

        if (queriedClient != currentClient)
        {
            ICollection<OrderCart> carts = queriedClient.orderCarts;
            int count = carts.Count + 1;
            string suffix = count % 2 == 0 ? "nd" : "st";
            string cartName = $"{queriedClient.username}'s {count}{suffix} Order Cart";
            cart.client = queriedClient;
            context.Entry(cart).Property(cart => cart.cart_name).CurrentValue = cartName;
        }
        
        // Editing Confirmed and Registered Date
        // For date in DTO, we format it to YYYY-MM-DD
        // Assuming that the frontend date is formatted to MM-DD-YYYY
        string[] dateArray = dto.dateRegistered.Split("-");

        int dtoMonth = Convert.ToInt32(dateArray[0]);
        int dtoDay = Convert.ToInt32(dateArray[1]);
        int dtoYear = Convert.ToInt32(dateArray[2]);

        DateTime dateNow = DateTime.Now;
        DateTime newRegisteredDate = new DateTime(dtoYear, dtoMonth, dtoDay);

        bool invalidAge = newRegisteredDate > dateNow;
 
        if (invalidAge == true)
        {
            result["Result"] = "Registered date cannot be later than the current moment";
            return result;
        }

        // Confirmed Date Editing
        if (cart.dateConfirmed != null)
        {
            string[] confirmedArray = dto.dateConfirmed.Split("-");
            int confirmedMonth = Convert.ToInt32(confirmedArray[0]);
            int confirmedDay = Convert.ToInt32(confirmedArray[1]);
            int confirmedYear = Convert.ToInt32(confirmedArray[2]);
            DateTime newConfirmedDate = new DateTime(confirmedYear, confirmedMonth, confirmedDay);
            if (newConfirmedDate != cart.dateConfirmed)
            {
                context.Entry(cart).Property(cart => cart.dateConfirmed).CurrentValue = newConfirmedDate;
            }
        }

        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        OrderCart toBeDeleted = context.OrderCarts.Include("shoeOrders").Where(c => c.Id == id).SingleOrDefault();
        ICollection<ShoeOrder> orders = toBeDeleted.shoeOrders;

        context.ShoeOrders.RemoveRange(orders);

        context.OrderCarts.Remove(toBeDeleted);

        await context.SaveChangesAsync();
    }
}