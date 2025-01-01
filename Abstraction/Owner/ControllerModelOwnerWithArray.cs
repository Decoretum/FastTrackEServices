namespace FastTrackEServices.Abstraction;

using FastTrackEServices.Data;
using FastTrackEServices.DTO;
using FastTrackEServices.HelperAlgorithms;
using FastTrackEServices.Implementation;
using Microsoft.AspNetCore.Mvc;

abstract public class ControllerModelOwnerWithArray : ControllerBase {

    private readonly AppDbContext context;

    protected readonly IEnumerable<IRestOperation> services;

    protected readonly IEnumerable<IRestOperation> restOperation;
    
    public ControllerModelOwnerWithArray (AppDbContext context, IEnumerable<IRestOperation> services)
    {
        this.context = context;
        this.services = services;
    }

    abstract public Task<IActionResult>  Post(Object dto);

    abstract public Task<IActionResult> GetAll();

    abstract public Task<IActionResult> Get(int id);

    abstract public Task<IActionResult> Put(Object dto, ITransform transform);

    abstract public Task<IActionResult> Delete(int id);
}