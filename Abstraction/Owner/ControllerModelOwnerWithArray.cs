namespace FastTrackEServices.Abstraction;

using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.Implementation;
using Microsoft.AspNetCore.Mvc;

public class ControllerModelOwnerWithArray : ControllerBase {
    IGet get;
    IPut put;
    IDelete delete;
    IPost post;
    private readonly AppDbContext context;

    public ControllerModelOwnerWithArray (IGet get, IPost post, AppDbContext context)
    {
        this.context = context;
        this.get = get;
        this.post = post;
    }

    async public Task<IActionResult>  Post(IDTO dto, string entityName)
    {
        this.post.post(context, dto);
        return StatusCode(201, new {date = $"{entityName} has been successfully created!"});
    }
    async public Task<IActionResult> GetAll()
    {
        Dictionary<string, object> result = await this.get.GetAll(this.context);
        return StatusCode(200, new { data = result });
    }

    async public Task<IActionResult> Get(int id, string entityName)
    {
        Object result = await this.get.Get(this.context, id);
        if (result == null)
        {
            return StatusCode(200, new { data = $"{entityName} with an ID of {id} is not found" });
        }
        return StatusCode(200, new {data = result});
    }

    // public ObjectResult
}