namespace NetFrontendDayWeb.Models;

public record SpeakerInfo
{
    public string Id { get; set; }
    public string Name { get; init; }
    public string Lastname { get; set; }
    public string Tagline { get; set; }
    public string Bio { get; init; }
    public string ProfilePicture { get; init; }
    public IEnumerable<Session> Sessions {get; init;}
    public IEnumerable<Link> Links { get; init; }
}
