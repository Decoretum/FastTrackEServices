using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class GetOrderCartNoOrders {

    public int Id {get; set;}

    public string clientName {get; set;}

    public string cart_name {get; set;}

    public string dateRegistered {get; set;}

    public string dateConfirmed {get; set;}
}