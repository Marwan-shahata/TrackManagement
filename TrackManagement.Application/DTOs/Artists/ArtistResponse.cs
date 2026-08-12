namespace TrackManagement.Application.DTOs.Artists;

public class ArtistResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Country { get; set; } = string.Empty;
}