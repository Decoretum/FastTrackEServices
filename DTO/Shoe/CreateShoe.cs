namespace FastTrackEServices.DTO;

public class CreateShoe : IDTO {
    public string name {get; set;}
    public string description {get; set;}
    public string brand {get; set;}
    public string[] shoeColors {get; set;}
}