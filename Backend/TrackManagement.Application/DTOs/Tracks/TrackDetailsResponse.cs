namespace TrackManagement.Application.DTOs.Tracks;

public class TrackDetailsResponse : TrackResponse
{
    public List<DistributionResponse> Distributions { get; set; }
        = new();
}