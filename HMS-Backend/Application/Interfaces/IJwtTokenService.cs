
using HMS_Backend.Domain.Entities;

namespace HMS_Backend.Application.Interfaces;


public interface IJwtTokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
    Guid? GetUserIdFromToken(string token);
}
 