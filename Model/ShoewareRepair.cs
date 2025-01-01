namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ShoewareRepair 
{
    [Key]
    public int Id {get; set;}

    [JsonIgnore]
    public virtual ICollection<OwnedShoeware> ownedShoes {get; set;}

    public Client client {get; set;}

    public DateTime dateRegistered {get; set;}

    public DateTime? dateConfirmed {get; set;}

}