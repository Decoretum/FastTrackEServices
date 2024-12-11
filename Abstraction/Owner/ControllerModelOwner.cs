namespace FastTrackEServices.Abstraction;

using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Implementation;
using Microsoft.AspNetCore.Mvc;

abstract public class ControllerModelOwner : ControllerBase {

    private readonly AppDbContext context;

    protected readonly IEnumerable<IRestOperation> services;

    protected readonly IRestOperation restOperation;
    
    public ControllerModelOwner (AppDbContext context, IEnumerable<IRestOperation> services)
    {
        this.context = context;
        this.services = services;
    }

    abstract public Task<IActionResult>  Post(Object dto);

    abstract public Task<IActionResult> GetAll();

    abstract public Task<IActionResult> Get(int id);

    abstract public Task<IActionResult> Put(Object dto, ITransform transform);

    abstract public Task<IActionResult> Delete(int id);




    // async public Task<IActionResult> GetAll()
    // {
    //     Dictionary<string, object> result = await this.get.GetAll(this.context);
    //     Console.WriteLine("Request Path: " + Request.Path);
    //     Console.WriteLine("Function called: " + this.get.GetType());
    //     return StatusCode(200, new { data = result });
    // }



    // async public Task<IActionResult> Get(int id, string entityType)
    // {
    //     Object result = await this.get.Get(this.context, id);
    //     if (result == null)
    //     {
    //         return StatusCode(200, new { data = $"{entityType} with an ID of {id} is not found" });
    //     }
    //     return StatusCode(200, new {data = result});
    // }



    // async public Task<IActionResult>  Post(Object dto, string entityName, string entityType)
    // {
    //     Dictionary<string, object> result = await this.restOperation.post();

    //     if (result["Result"] != "Success")
    //     return StatusCode(400, new {data = result});

    //     return StatusCode(201, new {data = $"{entityType} {entityName} has been successfully created!"});
    // }



    // async public Task<IActionResult> Put(Object dto, ITransform transform)
    // {
    //     Dictionary<string, object> result = await this.put.put(this.context, dto, transform);
    //     if (result["Result"] != "Success")
    //     return StatusCode(400, new {data = result});

    //     return StatusCode(200, new {data = result});
    // }



    // async public Task<IActionResult> Delete(int id)
    // {
    //     await this.delete.delete(this.context, id);
    //     return StatusCode(204);
    // }
}