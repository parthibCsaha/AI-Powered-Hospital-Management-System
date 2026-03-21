
using AutoMapper;
using HMS_Backend.Application.DTOs.Room;
using HMS_Backend.Application.Common;
using HMS_Backend.Domain.Entities;
using HMS_Backend.Application.Interfaces;

namespace HMS_Backend.Application.Services;



public class RoomService : IRoomService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
 
    public RoomService(IUnitOfWork uow, IMapper mapper) { _uow = uow; _mapper = mapper; }
 
    public async Task<IEnumerable<RoomResponseDto>> GetAllAsync(bool? availableOnly = null, CancellationToken ct = default)
    {
        var rooms = await _uow.Rooms.FindAsync(r => !r.IsDeleted && (availableOnly == null || r.IsOccupied != availableOnly), ct);
        return _mapper.Map<IEnumerable<RoomResponseDto>>(rooms.OrderBy(r => r.Floor).ThenBy(r => r.RoomNumber));
    }
 
    public async Task<RoomResponseDto?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct);
        return room is null ? null : _mapper.Map<RoomResponseDto>(room);
    }
 
    public async Task<RoomResponseDto> CreateAsync(CreateRoomDto dto, CancellationToken ct = default)
    {
        if (await _uow.Rooms.AnyAsync(r => r.RoomNumber == dto.RoomNumber && !r.IsDeleted, ct))
            throw new InvalidOperationException($"Room '{dto.RoomNumber}' already exists.");
 
        var room = _mapper.Map<Room>(dto);
        await _uow.Rooms.AddAsync(room, ct);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<RoomResponseDto>(room);
    }
 
    public async Task<RoomResponseDto> UpdateAsync(Guid id, UpdateRoomDto dto, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Room '{id}' not found.");
        _mapper.Map(dto, room);
        room.UpdatedAt = DateTime.UtcNow;
        _uow.Rooms.Update(room);
        await _uow.SaveChangesAsync(ct);
        return _mapper.Map<RoomResponseDto>(room);
    }
 
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        var room = await _uow.Rooms.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Room '{id}' not found.");

        if (room.IsOccupied) throw new InvalidOperationException("Cannot delete an occupied room.");
        
        _uow.Rooms.SoftDelete(room);
        await _uow.SaveChangesAsync(ct);
    }
}