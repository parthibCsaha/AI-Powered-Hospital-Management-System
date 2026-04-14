using HMS_Backend.Application.Common;
using HMS_Backend.Application.DTOs.Room;
using HMS_Backend.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;



namespace HMS_Backend.API.Controllers;


[Authorize]
[Route("api/v1/rooms")]
[ApiController]
[Produces("application/json")]
public class RoomsController : ControllerBase
{
    private readonly IRoomService _service;
    public RoomsController(IRoomService service) => _service = service;
 
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<RoomResponseDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] bool? availableOnly, CancellationToken ct)
        => Ok(ApiResponse<IEnumerable<RoomResponseDto>>.Ok(await _service.GetAllAsync(availableOnly, ct)));
 
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponse<RoomResponseDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken ct)
    {
        var result = await _service.GetByIdAsync(id, ct);
        return result is null ? NotFound(ApiResponse.Fail($"Room '{id}' not found.")) : Ok(ApiResponse<RoomResponseDto>.Ok(result));
    }
 
    [HttpPost]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<RoomResponseDto>), StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] CreateRoomDto dto, CancellationToken ct)
    {
        var result = await _service.CreateAsync(dto, ct);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, ApiResponse<RoomResponseDto>.Ok(result, "Room created."));
    }
 
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse<RoomResponseDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateRoomDto dto, CancellationToken ct)
        => Ok(ApiResponse<RoomResponseDto>.Ok(await _service.UpdateAsync(id, dto, ct), "Room updated."));
 
    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "Admin")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        await _service.DeleteAsync(id, ct);
        return Ok(ApiResponse.Ok("Room deleted."));
    }
}