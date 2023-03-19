using API.Errors;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class BuggyController : BaseApiController
{
    private readonly StoreContext _storeContext;

    public BuggyController(StoreContext storeContext)
    {
        _storeContext = storeContext;
    }
    
    
    [HttpGet("not-found")]
    public ActionResult GetNotFoundRequest()
    {
        var thing = _storeContext.Products.Find(80);
        if (thing == null)
            return NotFound(new ApiResponse(404));
        
        return Ok();
    }

    [HttpGet("server-error")]
    public ActionResult GetServerErrorRequest()
    {
        var thing = _storeContext.Products.Find(80);
        
        var thingToReturn = thing.ToString();

        return Ok();
    }
    
    [HttpGet("bad-request")]
    public ActionResult GetBadRequest()
    {
        return BadRequest(new ApiResponse(400));
    }
    
    [HttpGet("bad-request/{id}")]
    public ActionResult GetBadRequest(int id)
    {
        return Ok();
    }
    
    [HttpGet("endpoint-that-does-not-exist")]
    public ActionResult Get()
    {
        return Ok();
    }
}