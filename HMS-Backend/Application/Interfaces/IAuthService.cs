


using HMS_Backend.Application.DTOs.Auth;
 
namespace HMS_Backend.Application.Interfaces;


public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
    Task RevokeTokenAsync(Guid userId, CancellationToken cancellationToken = default);
    
}