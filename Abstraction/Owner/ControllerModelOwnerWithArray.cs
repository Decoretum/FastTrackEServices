namespace FastTrackEServices.Abstraction;

using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Implementation;
using Microsoft.AspNetCore.Mvc;

public class ControllerModelOwnerWithArray : ControllerBase {
    IGet get;
    IPut put;
    IDelete delete;
    IPost post;
    ITransform transform;
    private readonly AppDbContext context;

    public ControllerModelOwnerWithArray (ITransform transform, IGet get, IPost post, IPut put, AppDbContext context)
    {
        this.context = context;
        this.get = get;
        this.post = post;
        this.put = put;
    }

    async public Task<IActionResult>  Post(Object dto, string entityName, string entityType)
    {
        Dictionary<string, object> result = await this.post.post(context, dto);
        return StatusCode(201, new {data = $"{entityType} {entityName} has been successfully created!"});
    }
    async public Task<IActionResult> GetAll()
    {
        Dictionary<string, object> result = await this.get.GetAll(this.context);
        return StatusCode(200, new { data = result });
    }

    async public Task<IActionResult> Get(int id, string entityType)
    {
        Object result = await this.get.Get(this.context, id);
        if (result == null)
        {
            return StatusCode(200, new { data = $"{entityType} with an ID of {id} is not found" });
        }
        return StatusCode(200, new {data = result});
    }

    async public Task<IActionResult> Put(Object dto, ITransform transform)
    {
        Dictionary<string, object> result = await this.put.put(this.context, dto, transform);
        if (result["Result"] != "Success")
        return StatusCode(400, new {data = result});

        return StatusCode(200, new {data = result});
    }
}