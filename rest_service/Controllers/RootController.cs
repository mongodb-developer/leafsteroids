using Microsoft.AspNetCore.Mvc;

namespace RestService.Controllers;

[ApiController]
[Route("")]
public class RootsController : BaseController
{
    public RootsController(ILogger<RootsController> logger) : base(logger)
    {
    }

    [HttpGet(Name = "GetRoot")]
    public string GetRoot()
    {
        Logger.LogDebug($"Route {nameof(GetRoot)} called.");

        return "I'm alive!";
    }
}