using System.Collections;
using FastTrackEServices.Data;
using FastTrackEServices.Model;
using Microsoft.AspNetCore.Mvc;

namespace FastTrackEServices.Implementation;

public interface IGet 
{
    Task<Dictionary<string, object>>? GetAll(AppDbContext context);
    Task<Object>? Get(AppDbContext context, int id);
}