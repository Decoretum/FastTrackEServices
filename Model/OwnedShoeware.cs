namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class OwnedShoeware
{
    [Key]
    public int Id {get; set;}

    public virtual Client client {get; set;}

    [JsonIgnore]
    public virtual ShoewareRepair? shoewareRepair {get; set;}

    public virtual Shoeware shoe {get; set;}

    public DateTime dateAcquired {get; set;}
}