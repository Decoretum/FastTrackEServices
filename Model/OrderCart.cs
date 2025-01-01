namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class OrderCart
{
    [Key]
    public int Id {get; set;}

    public Client client {get; set;}

    [JsonIgnore]
    public ICollection<ShoeOrder>? shoeOrders {get; set;}

    [Column(TypeName="varchar(100)")]
    public string cart_name {get; set;}

    public DateTime dateRegistered {get; set;}

    public DateTime? dateConfirmed {get; set;}


}