using TrackManagement.Application.DTOs.Auth;

namespace TrackManagement.Application.Interfaces.Services;

public interface ITokenService
{
    TokenResponse GenerateToken(string username);
}