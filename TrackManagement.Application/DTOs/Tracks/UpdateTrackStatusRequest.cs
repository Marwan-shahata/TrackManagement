using System.ComponentModel.DataAnnotations;

namespace TrackManagement.Application.DTOs.Tracks;

public class UpdateTrackStatusRequest
{
    [Required]
    public string Status { get; set; } = string.Empty;
}