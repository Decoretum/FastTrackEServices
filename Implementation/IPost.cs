using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FastTrackEServices.Implementation;

public interface IPost {
    Task<Dictionary<string, object>> post(AppDbContext context, Object dto);
}