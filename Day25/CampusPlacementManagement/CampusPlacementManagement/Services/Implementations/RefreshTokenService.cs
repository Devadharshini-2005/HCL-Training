using CampusPlacementManagement.Data;
using CampusPlacementManagement.Models;
using CampusPlacementManagement.Models.Identity;
using CampusPlacementManagement.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CampusPlacementManagement.Services.Implementations;

public class RefreshTokenService : IRefreshTokenService
{
    private readonly ApplicationDbContext _context;

    public RefreshTokenService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> CreateRefreshTokenAsync(ApplicationUser user)
    {
        var refreshToken = GenerateRefreshToken();

        var tokenEntity = new RefreshToken
        {
            UserId = user.Id,
            TokenHash = HashToken(refreshToken),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _context.RefreshTokens.AddAsync(tokenEntity);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<ApplicationUser?> ValidateRefreshTokenAsync(
        string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);

        var tokenEntity = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash &&
                rt.RevokedAt == null &&
                rt.ExpiresAt > DateTime.UtcNow);

        return tokenEntity?.User;
    }

    public async Task<(ApplicationUser? User, string? NewRefreshToken)>
        RotateRefreshTokenAsync(string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);

        await using var transaction =
            await _context.Database.BeginTransactionAsync();

        var oldToken = await _context.RefreshTokens
            .Include(rt => rt.User)
            .FirstOrDefaultAsync(rt => rt.TokenHash == tokenHash);

        // Check whether the old token is valid.
        if (oldToken == null ||
            oldToken.RevokedAt != null ||
            oldToken.ExpiresAt <= DateTime.UtcNow)
        {
            return (null, null);
        }

        // Generate a new refresh token.
        var newRefreshToken = GenerateRefreshToken();
        var newTokenHash = HashToken(newRefreshToken);

        // Revoke the old token.
        oldToken.RevokedAt = DateTime.UtcNow;
        oldToken.ReplacedByTokenHash = newTokenHash;

        // Store the new token.
        var newToken = new RefreshToken
        {
            UserId = oldToken.UserId,
            TokenHash = newTokenHash,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        };

        await _context.RefreshTokens.AddAsync(newToken);

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();

        return (oldToken.User, newRefreshToken);
    }

    public async Task RevokeRefreshTokenAsync(string refreshToken)
    {
        var tokenHash = HashToken(refreshToken);

        var tokenEntity = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt =>
                rt.TokenHash == tokenHash &&
                rt.RevokedAt == null);

        if (tokenEntity == null)
            return;

        tokenEntity.RevokedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
    }

    private static string GenerateRefreshToken()
    {
        var randomBytes = RandomNumberGenerator.GetBytes(64);

        return Convert.ToBase64String(randomBytes);
    }

    private static string HashToken(string token)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(token);

        var hashBytes = SHA256.HashData(tokenBytes);

        return Convert.ToBase64String(hashBytes);
    }
}