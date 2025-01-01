using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class GetShoeRepair {

    public int Id {get; set;}
    public int[] ownedShoes {get; set;}

    public Client client {get; set;}

    public string dateRegistered {get; set;}

    public string dateConfirmed {get; set;}
}
