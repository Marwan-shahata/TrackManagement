using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs.Auth;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ITokenService _tokenService;

    public AuthController(ITokenService tokenService)
    {
        _tokenService = tokenService;
    }

    [HttpPost("token")]
    public ActionResult<TokenResponse> GetToken(
        LoginRequest request)
    {
        if (request.Username != "admin" ||
            request.Password != "Admin123!")
        {
            return Unauthorized();
        }

        var token =
            _tokenService.GenerateToken(
                request.Username);

        return Ok(token);
    }
}