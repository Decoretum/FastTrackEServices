namespace FastTrackEServices.DTO;

public class EditShoeOrder {
    public int shoeOrderId {get; set;}
    
    public int shoeId {get; set;}

    public int orderCartId {get; set;}

    public int quantity {get; set;}

    public string shoeColor {get; set;}
}