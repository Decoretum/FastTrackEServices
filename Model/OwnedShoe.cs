namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class OwnedShoe
{
    [Key]
    public int Id {get; set;}

    public virtual Client client {get; set;}

    [JsonIgnore]
    public virtual ShoeRepair? shoeRepair {get; set;}

    public virtual Shoe shoe {get; set;}
}