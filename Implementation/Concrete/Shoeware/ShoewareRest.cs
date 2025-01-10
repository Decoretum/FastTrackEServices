using System.Text.Json;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Model;
using Microsoft.EntityFrameworkCore;

namespace FastTrackEServices.Implementation;

public class ShoewareRest : IRestOperation {

    public async Task<Dictionary<string, object>>? GetAll(AppDbContext context)
    {
        object result;
        ICollection<Shoeware> shoes = await context.Shoewares.Include("shoeColors").ToListAsync();
        Dictionary<string, object> keyValue = new();
        if (shoes.Count == 0)
        {
            result = "There are no Shoes stored in the application";
            keyValue["Result"] = result;
            return keyValue;
        } else {
            object[] shoeArray = new object[shoes.Count];
            int j = 0;
            foreach (Shoeware s in shoes)
            {
                GetShoe shoe = new()
                {
                    id = s.Id,
                    name = s.name,
                    brand = s.brand,
                    description = s.description
                };
                string[] colors = new string[s.shoeColors.Count];
                int i = 0;
                foreach (ShoewareColor color in s.shoeColors)
                {
                    colors[i] = color.name;
                    i++;
                }
                shoe.shoeColors = colors;
                shoeArray[j] = shoe;
                j++;
            }
            result = shoeArray;
            keyValue["Result"] = result;
        }
        return keyValue;
    }

    public async Task<Object>? Get(AppDbContext context, int id)
    {
        Shoeware? shoeware = await context.Shoewares.Include("shoeColors").Where(x => x.Id == id).SingleOrDefaultAsync();
        return shoeware;
    }

    public async Task<Dictionary<string, object>> Post(AppDbContext context, Object idto)
    {
            CreateShoe dto = JsonSerializer.Deserialize<CreateShoe>(idto.ToString());
            Dictionary<string, object> result = new();

            Shoeware checkExisting = await context.Shoewares.Where(shoe => shoe.name == dto.name).SingleOrDefaultAsync();

            // Constraints
            if (dto.name.Length <= 5)
            {
                result["Result"] = "The Shoe name must be greater than 5 characters";
                return result;
            } else if (checkExisting != null)
            {
                result["Result"] = $"There is already an existing shoe with a name of \"{dto.name}\"";
                return result;
            }

            
            Shoeware shoe = new ()
            {
                name = dto.name,
                brand = dto.brand,
                description = dto.description,
                stock = dto.stock
            };

            ShoewareColor[] shoeColors = new ShoewareColor[dto.shoeColors.Length];
            for (int i = 0; i <= dto.shoeColors.Length - 1; i++)
            {
                string dtoColorName = dto.shoeColors[i];
                ShoewareColor color = new()
                {
                    name = dtoColorName,
                    shoe = shoe
                };
                shoeColors[i] = color;
            }
            context.ShoewareColors.AddRange(shoeColors);
            context.Shoewares.Add(shoe);
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task<Dictionary<string, object>> Put(AppDbContext context, Object idto, ITransform transform)
    {
        //Constraints
            CollectionToStringArray transformArray = (CollectionToStringArray) transform;
            Dictionary<string, object> result = new();
            EditShoe? dto = JsonSerializer.Deserialize<EditShoe>(idto.ToString());
            List<Shoeware> checkExisting = await context.Shoewares.Where(shoe => shoe.name == dto.name).ToListAsync();
            bool sameShoeName = (await context.Shoewares?.Where(shoe => shoe.Id == dto.Id).SingleOrDefaultAsync()).name == dto?.name;

            if (dto.name.Length <= 5)
            {
                result["Result"] = "The Shoe name must be greater than 5 characters";
                return result;
            } else if (checkExisting.Count >= 1 && sameShoeName == false)
            {
                result["Result"] = $"There is already an existing shoe with a name of {dto.name}";
                return result;
            }
            Shoeware toBeEdited = await context.Shoewares.Include("shoeColors").Where(shoe => shoe.Id == dto.Id).SingleOrDefaultAsync();
            ICollection<ShoewareColor> colors = toBeEdited.shoeColors; //error

            //Shoe Colors
            string[] shoeColors = transformArray.ConvertCollection<ShoewareColor>((ICollection<ShoewareColor>) colors); 
            string[] dtoColors = dto.shoeColors;
            

            List<string> removedColors = new ();
            List<ShoewareColor> newColors = new ();
            bool brandNewColor = false;
            bool removeColor = false;

            // Removing Color children from shoe model
            foreach (string color in shoeColors)
            {
                if (!dtoColors.Contains(color))
                {
                    removedColors.Add(color);
                    removeColor = true;
                }
            }

            // Adding new Color Children to Shoe Model
            foreach (string color in dtoColors)
            {
                if (!shoeColors.Contains(color))
                {
                    ShoewareColor newColor = new ()
                    {
                        name = color,
                        shoe = toBeEdited
                    };
                    brandNewColor = true;
                    newColors.Add(newColor);
                }
            }

            // DB Operation remove
            if (removeColor == true)
            {
                List<ShoewareColor> queriedColors = context.ShoewareColors.Where(
                color => removedColors.Contains(color.name) == true
                && 
                color.shoe == toBeEdited
                ).ToList();

                context.ShoewareColors.RemoveRange(queriedColors);
            }

            // DB Operation add
            if (brandNewColor == true)
            context.ShoewareColors.AddRange(newColors);


            // Editing other Shoe Properties
            context.Entry(toBeEdited).Property(color => color.name).CurrentValue = dto.name;
            context.Entry(toBeEdited).Property(color => color.brand).CurrentValue = dto.brand;
            context.Entry(toBeEdited).Property(color => color.description).CurrentValue = dto.description;
            context.Entry(toBeEdited).Property(color => color.stock).CurrentValue = dto.stock;
            
            await context.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

    public async Task Delete(AppDbContext context, int id)
    {
        Shoeware toBeDeleted = await context.Shoewares.Include("shoeColors").Include("shoewareOrders").Where(shoe => shoe.Id == id).SingleOrDefaultAsync();
        ICollection<ShoewareColor> colors = toBeDeleted.shoeColors;
        ICollection<ShoewareOrder> orders = toBeDeleted.shoeOrders;

        // Delete ShoeColors
        context.ShoewareColors.RemoveRange(colors);

        // Delete Shoe
        context.Shoewares.Remove(toBeDeleted);

        // Delete Shoeware Orders
        context.ShoewareOrders.RemoveRange(orders);

        // Delete OwnedShoe
        await context.SaveChangesAsync();
    }
}