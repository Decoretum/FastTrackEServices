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

public class ShoePut : IPut
{
    public async Task<Dictionary<string, object>> put(AppDbContext appDbContext, Object idto, ITransform transform)
    {
            //Constraints
            CollectionToStringArray transformArray = (CollectionToStringArray) transform;
            Dictionary<string, object> result = new();
            EditShoe? dto = JsonSerializer.Deserialize<EditShoe>(idto.ToString());
            List<Shoe> checkExisting = await appDbContext.Shoes.Where(shoe => shoe.name == dto.name).ToListAsync();
            bool sameShoe = (await appDbContext.Shoes?.Where(shoe => shoe.Id == dto.Id).SingleOrDefaultAsync()).name == dto?.name;
            Console.WriteLine(sameShoe);
            if (dto.name.Length <= 5)
            {
                result["Result"] = "The Shoe name must be greater than 5 characters";
                return result;
            } else if (checkExisting.Count >= 2 && sameShoe == false)
            {
                result["Result"] = $"There is already an existing shoe with a name of {dto.name}";
                return result;
            }
            Shoe toBeEdited = await appDbContext.Shoes.Include("shoeColors").Where(shoe => shoe.Id == dto.Id).SingleOrDefaultAsync();
            ICollection<ShoeColor> colors = toBeEdited.shoeColors; //error

            //Shoe Colors
            string[] shoeColors = transformArray.ConvertCollection<ShoeColor>((ICollection<ShoeColor>) colors); 
            string[] dtoColors = dto.shoeColors;
            

            List<string> removedColors = new ();
            List<ShoeColor> newColors = new ();
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
                    ShoeColor newColor = new ()
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
                List<ShoeColor> queriedColors = appDbContext.ShoeColors.Where(
                color => removedColors.Contains(color.name) == true
                && 
                color.shoe == toBeEdited
                ).ToList();

                appDbContext.ShoeColors.RemoveRange(queriedColors);
            }

            // DB Operation add
            if (brandNewColor == true)
            appDbContext.ShoeColors.AddRange(newColors);


            // Editing other Shoe Properties
            appDbContext.Entry(toBeEdited).Property(color => color.name).CurrentValue = dto.name;
            appDbContext.Entry(toBeEdited).Property(color => color.brand).CurrentValue = dto.brand;
            appDbContext.Entry(toBeEdited).Property(color => color.description).CurrentValue = dto.description;

            await appDbContext.SaveChangesAsync();
            result["Result"] = "Success";
            return result;
    }

}