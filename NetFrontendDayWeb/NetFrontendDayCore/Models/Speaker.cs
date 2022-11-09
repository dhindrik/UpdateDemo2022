namespace NetFrontendDayWeb.Models;

public record Speaker
{
    public string Id { get; set; }
    public string Fullname { get; init; }
    public string Firstname { get; set; }
    public string Lastname { get; set; }
    public string ProfilePicture { get; init; }
    public string Tagline { get; set; }
    public string Bio { get; init; }
    public Session[] Sessions {get;init;}
    public Link[] Links { get; init; }
}
