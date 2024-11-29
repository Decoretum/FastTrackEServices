using FastTrackEServices.Data;

namespace FastTrackEServices.Implementation;

public interface IDelete {
    Task delete(AppDbContext context, int id);
}