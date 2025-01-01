using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class GetOwnedShoe {

    public int id {get; set;}
    
    public Client client {get; set;}

    public GetShoe shoe {get; set;}

    public int shoeRepairId {get; set;}

    public string dateAcquired {get; set;}
}