namespace TrackManagement.Application.DTOs.Tracks;

public class TrackResponse
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public int ArtistId { get; set; }

    public string ArtistName { get; set; } = string.Empty;

    public string Isrc { get; set; } = string.Empty;

    public DateOnly ReleaseDate { get; set; }

    public string Genre { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;
}