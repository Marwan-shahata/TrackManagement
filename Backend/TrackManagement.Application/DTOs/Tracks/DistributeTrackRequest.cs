using System.ComponentModel.DataAnnotations;

namespace TrackManagement.Application.DTOs.Tracks;

public class DistributeTrackRequest
{
    [Required]
    [MinLength(1)]
    public List<int> DspIds { get; set; } = new();
}