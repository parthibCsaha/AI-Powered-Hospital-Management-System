


using HMS_Backend.Application.DTOs.Auth;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Domain.Entities;


namespace HMS_Backend.Infrastructure.Services;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtTokenService _jwtTokenService;
    public AuthService(IUnitOfWork unitOfWork, IJwtTokenService jwtTokenService)
    {
        _unitOfWork = unitOfWork;
        _jwtTokenService = jwtTokenService;
    }
    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email, cancellationToken
        ) ?? throw new UnauthorizedAccessException("Invalid email or password.");

        if (!user.IsActive)
            throw new UnauthorizedAccessException("Your account has been deactivated. Please contact support.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedAccessException("Invalid email or password.");

        return await GenerateAuthResponseAsync(user, cancellationToken);

    }    

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var existingUser = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Email == request.Email, cancellationToken
        );

        if (existingUser is not null)
            throw new InvalidOperationException("Email is already registered.");

        var user = new User
        {
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
            FirstName = request.FirstName,
            LastName = request.LastName,
            Role = Domain.Enums.UserRole.Patient, // Public registration is always Patient
            IsActive = true
        };

        await _unitOfWork.Users.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await GenerateAuthResponseAsync(user, cancellationToken);

    }

    public async Task<AuthResponseDto> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default)
    {
        var userId = _jwtTokenService.GetUserIdFromToken(request.AccessToken);
        if (userId is null)
            throw new UnauthorizedAccessException("Invalid access token.");

        var user = await _unitOfWork.Users.FirstOrDefaultAsync(
            u => u.Id == userId && !u.IsDeleted, cancellationToken
            ) ?? throw new UnauthorizedAccessException("User not found.");

        if (user.RefreshToken != request.RefreshToken)
            throw new UnauthorizedAccessException("Invalid refresh token.");

        if (user.RefreshTokenExpiry < DateTime.UtcNow)
            throw new UnauthorizedAccessException("Refresh token has expired. Please log in again.");

        return await GenerateAuthResponseAsync(user, cancellationToken);

    }


    public async Task RevokeTokenAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.Users.FirstOrDefaultAsync(u => u.Id == userId && !u.IsDeleted, cancellationToken)
            ?? throw new KeyNotFoundException("User not found.");
 
        user.RefreshToken = null;
        user.RefreshTokenExpiry = null;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private async Task<AuthResponseDto> GenerateAuthResponseAsync(User user, CancellationToken cancellationToken)
    {
        var accessToken = _jwtTokenService.GenerateAccessToken(user);
        var refreshToken = _jwtTokenService.GenerateRefreshToken();
 
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        user.LastLoginAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;
        _unitOfWork.Users.Update(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
 
        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Email = user.Email,
            FullName = $"{user.FirstName} {user.LastName}",
            Role = user.Role.ToString()
        };
    }

}