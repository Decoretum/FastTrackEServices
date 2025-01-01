namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


public class Client 
{
    [Key]
    public int Id {get; set;}

    [JsonIgnore]
    public virtual ICollection<OwnedShoe>? ownedShoes {get; set;}

    [JsonIgnore]
    public virtual ICollection<OrderCart>? orderCarts {get; set;}

    [JsonIgnore]
    public virtual ICollection<ShoeRepair>? shoeRepairs {get; set;}

    [Column(TypeName="varchar(100)")]
    public string username {get; set;}

    public DateTime dateOfBirth {get; set;}

    [Column(TypeName="varchar(150)")]
    public string location {get; set;}

    [Column(TypeName="varchar(11)")]
    public string contactNumber {get; set;}

}