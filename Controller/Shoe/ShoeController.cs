namespace Controller;
using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Abstraction;
using FastTrackEServices.Implementation;
using System.Text.Json;
using FastTrackEServices.HelperAlgorithms;

[Route("api/[controller]")]
[ApiController]
public class ShoeController : ControllerModelOwner {
    private const string constantIndividualPath = "[action]/{id:int}";

    private readonly AppDbContext context;

    protected readonly IEnumerable<IRestOperation> services;

    protected readonly IRestOperation restOperation;

    public ShoeController(AppDbContext context, IEnumerable<IRestOperation> services) : base (context, services)
    {
        this.context = context;
        this.services = services;
        this.restOperation = services.FirstOrDefault(s => s.GetType() == typeof(ShoeRest));
    }

    [HttpGet("[action]")]
    async public override Task<IActionResult> GetAll()
    {

        Dictionary<string, object> result = await this.restOperation.GetAll(this.context);
        return StatusCode(200, new { data = result });
    }

    [HttpGet($"{constantIndividualPath}")]
    async public override Task<IActionResult> Get([FromRoute] int id)
    {  
        Object result = await this.restOperation.Get(this.context, id); 
        string entityType = this.ControllerContext.RouteData.Values["controller"].ToString();
        if (result == null)
        {
            return StatusCode(200, new { data = $"{entityType} with an ID of {id} is not found" });
        }
        return StatusCode(200, new {data = result});
    }

    [HttpPost("[action]")]
    async public override Task<IActionResult> Post([FromBody] Object dto)
    {
        try {
            string clientType = this.ControllerContext.RouteData.Values["controller"].ToString();
            string clientName = JsonSerializer.Deserialize<CreateShoe>(dto.ToString()).name;

            Dictionary<String, Object> result = await this.restOperation.Post(this.context, dto);

            if (result["Result"] == "Success")
            return StatusCode(201, new {data = $"{clientType} {clientName} has been successfully created!"});

            else
            return StatusCode(400, new {data = result});


        } 
        
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new {data = ex.InnerException.Message});
        }
    }

    [HttpPut("[action]")]
    async public override Task<IActionResult> Put([FromBody] Object dto, ITransform transform)
    {
        // Assuming that the DTO's shoeColor array contains a hashmap of <id - int, string - name>
        // Frontend returns the original id for a shoecolor, but possibly with a different name
        try {
            Dictionary<String, Object> result = this.restOperation.Put(this.context, dto, transform).Result;
            
            if (result["Result"] != "Success")
            return StatusCode(400, new {data = result});

            return StatusCode(200, new {data = result});
        }

        catch (DbUpdateException ex)
        {
            return StatusCode(200, new {data = ex.InnerException.Message});
        }

    }

    [HttpDelete($"{constantIndividualPath}")]
    async public override Task<IActionResult> Delete([FromRoute] int id)
    {
        try {
             await this.restOperation.Delete(this.context, id);
             return StatusCode(204);
        } catch (DbUpdateException ex)
        {
            return StatusCode(500, new {data = ex.InnerException.Message});
        }
    }
}

