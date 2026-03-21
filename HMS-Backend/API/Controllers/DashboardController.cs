
using HMS_Backend.Application.Common;

using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HMS_Backend.API.Controllers;


[Authorize(Roles = "Admin,Doctor,Receptionist")]
[Route("api/v1/dashboard")]
[ApiController]
[Produces("application/json")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _service;
    public DashboardController(IDashboardService service) => _service = service;
 
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<DashboardStatsDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStats(CancellationToken ct)
        => Ok(ApiResponse<DashboardStatsDto>.Ok(await _service.GetStatsAsync(ct)));
}