using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FastTrackEServices.Implementation;

public class ShoewareOrderRest : IRestOperation {

    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<ShoewareOrder> orders = await context.ShoewareOrders.Include("shoe").Include("orderCart").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (orders.Count == 0)
        {
            result = "There are no Shoe Orders stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] orderArray = new object[orders.Count];
            int j = 0;
            foreach (ShoewareOrder order in orders)
            {
                Shoeware shoe = context.Shoewares.Where(s => s.Id == order.shoeware.Id).Include("shoeColors").SingleOrDefault();
                GetShoeNoColors shoeDto = new GetShoeNoColors()
                {
                    id = shoe.Id,
                    name =  shoe.name,
                    brand = shoe.brand,
                    description = shoe.description
                };

                string newConfirmed;

                OrderCart cart = context.OrderCarts.Where(cart => cart.Id == order.orderCart.Id).Include("client").SingleOrDefault();
                GetOrderCartNoOrders cartDto = new()
                {
                    Id = order.orderCart.Id,
                    clientName = cart.client.username,
                    cart_name = order.orderCart.cart_name,
                    dateRegistered = order.orderCart.dateRegistered.ToShortDateString(),
                };

                if (order.orderCart.dateConfirmed == null)
                newConfirmed = "Not Yet Confirmed";

                else
                newConfirmed = ((DateTime) order.orderCart.dateConfirmed).ToShortDateString();

                cartDto.dateConfirmed = newConfirmed;

                GetShoeOrder orderDto = new GetShoeOrder()
                {
                    id = order.Id,
                    orderCart = cartDto,
                    shoe = shoeDto,
                    quantity = order.quantity,
                    shoeColor = order.shoeColor
                };

                orderArray[j] = orderDto;
                j++;
            }
            result = orderArray;
            keyValue["Result"] = result;
        }
        return keyValue;
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        ShoewareOrder order = await context.ShoewareOrders.Where(s => s.Id == id).Include("orderCart").Include("shoe").SingleOrDefaultAsync();
        Shoeware shoe = await context.Shoewares.Where(s => s.Id == order.shoeware.Id).Include("shoeColors").SingleOrDefaultAsync();
        
        GetShoeNoColors shoeDto = new GetShoeNoColors()
        {
            id = shoe.Id,
            name =  shoe.name,
            brand = shoe.brand,
            description = shoe.description
        };

        string newConfirmed;

        OrderCart cart = context.OrderCarts.Where(cart => cart.Id == order.orderCart.Id).Include("client").SingleOrDefault();
        GetOrderCartNoOrders cartDto = new()
        {
            Id = order.orderCart.Id,
            clientName = cart.client.username,
            cart_name = order.orderCart.cart_name,
            dateRegistered = order.orderCart.dateRegistered.ToShortDateString(),
        };

        if (order.orderCart.dateConfirmed == null)
        newConfirmed = "Not Yet Confirmed";

        else
        newConfirmed = ((DateTime) order.orderCart.dateConfirmed).ToShortDateString();

        cartDto.dateConfirmed = newConfirmed;

        GetShoeOrder orderDto = new GetShoeOrder()
        {
            id = order.Id,
            orderCart = cartDto,
            shoe = shoeDto,
            quantity = order.quantity,
            shoeColor = order.shoeColor
        };

        return orderDto;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
    {
        Dictionary<string, object> result = new();
        
        CreateShoeOrder dto = JsonSerializer.Deserialize<CreateShoeOrder>(idto.ToString());
        OrderCart orderCart =  context.OrderCarts.Where(oc => oc.Id == dto.orderCartId).SingleOrDefault();
        Shoeware shoe = context.Shoewares.Where(s => s.Id == dto.shoeId).SingleOrDefault();
        
        if (orderCart == null) 
        {
            result["Result"] = $"There is no corresponding Order Cart with an ID of {dto.orderCartId}";
            return result;
        }  
        
        if (shoe == null) {
            result["Result"] = $"There is no corresponding Shoe with a shoe ID of {dto.shoeId}";
            return result;
        } 

        ShoewareOrder order = new()
        {
            shoeware = shoe,
            orderCart = orderCart,
            quantity = dto.quantity,
            shoeColor = dto.shoeColor
        };


        context.ShoewareOrders.Add(order);
        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
        
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        Dictionary<string, Object> result = new();
        CollectionToStringArray transformEntity = (CollectionToStringArray) transform;
        EditShoeOrder dto = JsonSerializer.Deserialize<EditShoeOrder>(idto.ToString());
        ShoewareOrder order = await context.ShoewareOrders.Where(o => o.Id == dto.shoeOrderId).Include("shoe").Include("orderCart").SingleOrDefaultAsync();

        // Change References / Foreign Entities
        if (order.shoeware.Id != dto.shoeId)
        {
            Shoeware shoe = await context.Shoewares.Include("shoeColors").Where(s => s.Id == dto.shoeId).SingleOrDefaultAsync();
            order.shoeware = shoe;
        }

        if (order.orderCart.Id!= dto.orderCartId)
        {
            OrderCart cart = await context.OrderCarts.Where(oc => oc.Id == dto.orderCartId).SingleOrDefaultAsync();
            order.orderCart = cart;
        }

        // Change Properties
        Shoeware queriedShoe = await context.Shoewares.Where(s => s.Id == dto.shoeId).Include("shoeColors").SingleOrDefaultAsync();
        string[] colors = transformEntity.ConvertCollection<ShoewareColor>((ICollection<ShoewareColor>) queriedShoe.shoeColors);
        
        if (colors.Contains(dto.shoeColor))
        context.Entry(order).Property(o => o.shoeColor).CurrentValue = dto.shoeColor;

        else
        {
            string res = "'" + dto.shoeColor + "'" + " is not contained in the shoe colors of shoe " + "'" + queriedShoe.name + "'";
            result["Result"] = res;
            return result;
        }

        if (queriedShoe.stock >= dto.quantity)
        context.Entry(order).Property(o => o.quantity).CurrentValue = dto.quantity;

        else
        {
            int diff = dto.quantity - queriedShoe.stock;
            result["Result"] = $"Order Quantity of {dto.quantity} is {diff} times more than the available stock for the shoe named {queriedShoe.name}";
        }
        
        await context.SaveChangesAsync();
        result["Result"] = "Success";
        return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        OwnedShoeware toBeDeleted = await context.OwnedShoewares.Where(os => os.Id == id).SingleOrDefaultAsync();
        context.OwnedShoewares.Remove(toBeDeleted);
        await context.SaveChangesAsync();
    }
}