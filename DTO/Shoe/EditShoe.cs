namespace FastTrackEServices.DTO;

public class EditShoe : IDTO {

    public string name {get; set;}
    public string description {get; set;}
    public string brand {get; set;}
    public object[] shoeColors {get; set;}
}