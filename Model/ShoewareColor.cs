namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class ShoewareColor
{
    [Key]
    public int Id {get; set;}

    [JsonIgnore]
    public virtual Shoeware shoe {get; set;}

    [Column(TypeName="varchar(100)")]
    public string name {get; set;}

    public string ArrayName()
    {
        return name;
    }
}