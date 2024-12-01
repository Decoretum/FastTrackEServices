using FastTrackEServices.Model;

namespace FastTrackEServices.DTO;

public class CreateShoeRepair {

    public virtual ICollection<OwnedShoe> ownedShoes {get; set;}

    public int clientID {get; set;}

    public DateTime dateRegistered {get; set;}

    public DateTime? dateConfirmed {get; set;}
}

