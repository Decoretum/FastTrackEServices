namespace FastTrackEServices.DTO;

public class EditShoeRepair {

    public int repairId {get; set;}
    
    public int clientId {get; set;}

    public int[] ownedShoesArray {get; set;}

    public string dateRegistered {get; set;}

    public string dateConfirmed {get; set;}

    // "True" or "False"
    public string confirming {get; set;}

}