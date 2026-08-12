using Microsoft.AspNetCore.Mvc;
using TrackManagement.Application.DTOs.Tracks;
using TrackManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

namespace TrackManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TracksController : ControllerBase
{
    private readonly ITrackService _trackService;

    public TracksController(ITrackService trackService)
    {
        _trackService = trackService;
    }

    [HttpPost]
    public async Task<ActionResult<TrackResponse>> Create(
        CreateTrackRequest request,
        CancellationToken cancellationToken)
    {
        var track =
            await _trackService.CreateAsync(
                request,
                cancellationToken);

        return StatusCode(
            StatusCodes.Status201Created,
            track);
    }

    [HttpGet]
    public async Task<ActionResult<List<TrackResponse>>> GetAll(
        [FromQuery] int? artistId,
        [FromQuery] string? genre,
        [FromQuery] string? status,
        CancellationToken cancellationToken)
    {
        var tracks =
            await _trackService.GetAllAsync(
                artistId,
                genre,
                status,
                cancellationToken);

        return Ok(tracks);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TrackDetailsResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var track =
            await _trackService.GetByIdAsync(
                id,
                cancellationToken);

        if (track is null)
            return NotFound();

        return Ok(track);
    }
    [HttpPost("{id:int}/distribute")]
    public async Task<IActionResult> Distribute(
    int id,
    DistributeTrackRequest request,
    CancellationToken cancellationToken)
    {
        await _trackService.DistributeAsync(
            id,
            request,
            cancellationToken);

        return NoContent();
    }

    [Authorize]
    [HttpPatch("{id:int}/status")]
    public async Task<IActionResult> UpdateStatus(
    int id,
    UpdateTrackStatusRequest request,
    CancellationToken cancellationToken)
    {
        await _trackService.UpdateStatusAsync(
            id,
            request,
            cancellationToken);

        return NoContent();
    }

}