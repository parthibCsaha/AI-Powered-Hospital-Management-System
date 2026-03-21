
using HMS_Backend.Application.DTOs.Room;

namespace HMS_Backend.Application.Interfaces;

public interface IRoomService
{
    Task<IEnumerable<RoomResponseDto>> GetAllAsync(bool? availableOnly = null, CancellationToken ct = default);
    Task<RoomResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken ct = default);
    Task<RoomResponseDto> UpdateAsync(Guid id, UpdateRoomDto dto, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);


}