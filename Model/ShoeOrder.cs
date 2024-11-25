namespace FastTrackEServices.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class ShoeOrder 
{
    [Key]
    public int Id {get; set;}

    public Shoe shoe {get; set;}

    public OrderCart orderCart {get; set;}

    [Column(TypeName ="int")]
    public int quantity {get; set;}

    [Column(TypeName="varchar(50)")]
    public string shoeColor {get; set;}



}