using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FastTrackEServices.Abstraction;
using FastTrackEServices.Implementation;
using Implementation.Refined;

[Route("api/[controller]")]
[ApiController]
public class ShoeController : ControllerModelOwnerWithArray {
    private const string constantIndividualPath = "[action]/{id:int}";
    private readonly AppDbContext appDbContext;
    private readonly IGet get;
    private readonly IPost post;

    public ShoeController(IGet get, IPost post, AppDbContext context) : base (get, post, context)
    {
        appDbContext = context;
        this.get = new ShoeGet();
        this.post = new ShoePost();
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
    async public Task<IActionResult> MakeShoe([FromBody] IDTO dto)
    {

        try {
            
            Task<IActionResult> result = base.Post(dto, this.ControllerContext.RouteData.Values["controller"].ToString());
            return StatusCode(201, new {data = $"Shoe successfully created!"});
        } 
        
        catch (DbUpdateException ex)
        {
            return StatusCode(500, new {data = ex.InnerException.Message});
        }
    }

    // async public Task<IActionResult> EditShoe([FromBody] EditShoe shoe)
    // {
        // Assuming that the DTO's shoeColor array contains a hashmap of <id - int, string - name>
        // Frontend returns the original id for a shoecolor, but possibly with a different name


    // }
}

