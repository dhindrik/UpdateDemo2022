namespace NetFrontendDayWeb.Models;

public record Recording
{
    public string Speaker { get; init; }
    public string Session { get; init; }
    public string YouTubeUrl { get; init; }
}
