namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class Shoeware
{
    [Key]
    public int Id {get; set;}

    public virtual ICollection<ShoewareColor>? shoeColors {get; set;}

    [Column(TypeName="varchar(100)")]
    public string name {get; set;}

    [Column(TypeName="varchar(500)")]
    public string description {get; set;}

    [Column(TypeName="varchar(100)")]
    public string brand {get; set;}

    public int stock {get; set;}

}