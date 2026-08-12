using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs.Artists;
using TrackManagement.Application.Interfaces.Services;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistsController : ControllerBase
{
    private readonly IArtistService _artistService;

    public ArtistsController(IArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ArtistResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var artists =
            await _artistService.GetAllAsync(cancellationToken);

        return Ok(artists);
    }

    [HttpPost]
    public async Task<ActionResult<ArtistResponse>> Create(
        CreateArtistRequest request,
        CancellationToken cancellationToken)
    {
        var artist =
            await _artistService.CreateAsync(
                request,
                cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            artist);
    }
}