using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class GetOwnedShoe {

    public Client client {get; set;}

    public Shoe shoe {get; set;}

    public int shoeRepairId {get; set;}

    public string dateAcquired {get; set;}
}