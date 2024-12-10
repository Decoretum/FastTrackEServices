namespace Controller;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Abstraction;
using FastTrackEServices.Implementation;
using Implementation.Concrete;
using System.Text.Json;
using FastTrackEServices.HelperAlgorithms;
using System.Reflection;

[Route("api/[controller]")]
[ApiController]
public class ShoeController : ControllerModelOwnerWithArray {
    private const string constantIndividualPath = "[action]/{id:int}";

    private readonly AppDbContext appDbContext;

    private readonly IGet get;
    
    private readonly IPut put;

    private readonly IDelete delete;

    private readonly IPost post;

    private readonly ITransform transform;

    public ShoeController(ITransform transform, IGet get, IPost post, IPut put, IDelete delete, AppDbContext context) : base (transform, get, post, put, delete, context)
    {
        appDbContext = context;
        this.transform = new CollectionToStringArray();
        this.get = new ShoeGet();
        this.post = new ShoePost();
        this.delete = new ShoeDelete();
        this.put = new ShoePut();
    }

    [HttpGet("[action]")]
    public Task<IActionResult> GetAll()
    {
        return base.GetAll();
    }

    [HttpGet($"{constantIndividualPath}")]
    public Task<IActionResult> Get([FromRoute] int id)
    {  
        return base.Get(id, this.ControllerContext.RouteData.Values["controller"].ToString());
    }

    [HttpPost("[action]")]
    async public Task<IActionResult> Post([FromBody] Object dto)
    {
        try {
            string shoeType = this.ControllerContext.RouteData.Values["controller"].ToString();
            string shoeName = JsonSerializer.Deserialize<CreateShoe>(dto.ToString()).name;
            Task<IActionResult> result = base.Post(dto, shoeName, shoeType);
            return result.Result;
        } 
        
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new {data = ex.InnerException.Message});
        }
    }

    [HttpPut("[action]")]
    async public Task<IActionResult> Put([FromBody] Object dto)
    {
        // Assuming that the DTO's shoeColor array contains a hashmap of <id - int, string - name>
        // Frontend returns the original id for a shoecolor, but possibly with a different name
        try {
            Task<IActionResult> result = base.Put(dto, transform);
            return result.Result;
        }

        catch (DbUpdateException ex)
        {
            return StatusCode(200, new {data = ex.InnerException.Message});
        }

    }

    [HttpDelete($"{constantIndividualPath}")]
    async public Task<IActionResult> Delete([FromRoute] int id)
    {
        try {
             return await base.Delete(id);
        } catch (DbUpdateException ex)
        {
            return StatusCode(500, new {data = ex.InnerException.Message});
        }
    }
}

