using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
public abstract class BaseApiController : ControllerBase
{
    
}