using FastTrackEServices.Data;
using FastTrackEServices.HelperAlgorithms;

namespace FastTrackEServices.Implementation;

public interface IRestOperation {

    Task<Dictionary<string, object>>? GetAll(AppDbContext context);

    Task<Object>? Get(AppDbContext context, int id);

    Task<Dictionary<string, object>> Post(AppDbContext context, Object dto);

    Task<Dictionary<string, object>> Put(AppDbContext context, Object dto, ITransform transform);

    Task Delete(AppDbContext context, int id);
}