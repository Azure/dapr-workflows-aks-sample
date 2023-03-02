using Microsoft.AspNetCore.Mvc;

namespace WorkflowSample.Controllers;

[ApiController]
[Route("[controller]")]
public class GreetController : ControllerBase
{
    private readonly ILogger<GreetController> _logger;

    public GreetController(ILogger<GreetController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public String Get()
    {
        return "Hello World!";
    }
}
