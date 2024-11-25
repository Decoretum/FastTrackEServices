using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FastTrackEServices.Implementation;

public interface IPost {
    void post(AppDbContext context, IDTO dto);
}