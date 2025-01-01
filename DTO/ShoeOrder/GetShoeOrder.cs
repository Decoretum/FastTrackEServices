using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class GetShoeOrder {

    public int id {get; set;}
    
    public GetOrderCartNoOrders orderCart {get; set;}

    public GetShoeNoColors shoe {get; set;}

    public int quantity {get; set;}

    public string shoeColor {get; set;}
}