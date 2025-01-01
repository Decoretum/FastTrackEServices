namespace FastTrackEServices.DTO;

public class EditOrderCart {

    public int Id {get; set;}

    public string clientUsername {get; set;}

    public string dateRegistered {get; set;}

    public string dateConfirmed {get; set;}

    // "True" or "False"
    public string confirming {get; set;}
}