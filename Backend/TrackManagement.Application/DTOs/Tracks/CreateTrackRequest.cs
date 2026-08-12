using System.ComponentModel.DataAnnotations;

namespace TrackManagement.Application.DTOs.Tracks;

public class CreateTrackRequest
{
    [Required]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ArtistId { get; set; }

    [Required]
    [MaxLength(20)]
    public string Isrc { get; set; } = string.Empty;

    public DateOnly ReleaseDate { get; set; }

    [Required]
    [MaxLength(100)]
    public string Genre { get; set; } = string.Empty;
}