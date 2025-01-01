namespace FastTrackEServices.DTO;

public class GetClient {
    public int id {get; set;}
    public string username {get; set;}

    public string dateOfBirth {get; set;}

    public string location {get; set;}

    public string contactNumber {get; set;}

    public string[] ownedShoes {get; set;}
}