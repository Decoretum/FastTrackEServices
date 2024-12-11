// namespace Controller;
// using FastTrackEServices.Data;
// using FastTrackEServices.DTO;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using FastTrackEServices.Abstraction;
// using FastTrackEServices.Implementation;
// using Implementation.Concrete;
// using System.Text.Json;
// using FastTrackEServices.HelperAlgorithms;

// [Route("api/[controller]")]
// [ApiController]
// public class OwnedShoeController : ControllerModelOwner {
//     private const string constantIndividualPath = "[action]/{id:int}";

//     private readonly AppDbContext appDbContext;

//     private readonly IGet get;
    
//     private readonly IPut put;  

//     private readonly IDelete delete;

//     private readonly IPost post;

//     private readonly ITransform transform;

//     public OwnedShoeController(ITransform transform, IGet get, IPost post, IPut put, IDelete delete, AppDbContext context) : base (transform, get, post, put, delete, context)
//     {
//         this.appDbContext = context;
//         this.transform = new CollectionToStringArray();
//         this.get = new OwnedShoeGet();
//         this.post = new OwnedShoePost();
//         // this.delete = new ShoeRepairDelete();
//         // this.put = new ShoeRepairPut();
//     }

//     [HttpGet("[action]")]
//     public Task<IActionResult> GetAll()
//     {
//         return base.GetAll();
//     }

//     [HttpGet($"{constantIndividualPath}")]
//     public Task<IActionResult> Get([FromRoute] int id)
//     {  
//         return base.Get(id, this.ControllerContext.RouteData.Values["controller"].ToString());
//     }

//     [HttpPost("[action]")]
//     async public Task<IActionResult> Post([FromBody] Object dto)
//     {
//         try {
//             string shoeRepairType = this.ControllerContext.RouteData.Values["controller"].ToString();
//             string shoeRepairName = "\", A brand new owned shoe \"";
//             Task<IActionResult> result = base.Post(dto, shoeRepairName, shoeRepairType);
//             return result.Result;
//         } 
        
//         catch (DbUpdateException ex)
//         {
//             return StatusCode(500, new {data = ex.InnerException.Message});
//         }
//     }

//     [HttpPut("[action]")]
//     async public Task<IActionResult> Put([FromBody] Object dto)
//     {
//         // Assuming that the DTO's shoeColor array contains a hashmap of <id - int, string - name>
//         // Frontend returns the original id for a shoecolor, but possibly with a different name
//         try {
//             Task<IActionResult> result = base.Put(dto, transform);
//             return result.Result;
//         }

//         catch (DbUpdateException ex)
//         {
//             return StatusCode(200, new {data = ex.InnerException.Message});
//         }

//     }

//     [HttpDelete($"{constantIndividualPath}")]
//     async public Task<IActionResult> Delete([FromRoute] int id)
//     {
//         try {
//              return await base.Delete(id);
//         } catch (DbUpdateException ex)
//         {
//             return StatusCode(500, new {data = ex.InnerException.Message});
//         }
//     }
// }

