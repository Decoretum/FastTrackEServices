using FastTrackEServices.Data;
using FastTrackEServices.HelperAlgorithms;
using Microsoft.AspNetCore.Mvc;

namespace FastTrackEServices.Implementation;

public interface IPut {
    Task<Dictionary<string, object>> put(AppDbContext context, Object dto, ITransform transform);
}