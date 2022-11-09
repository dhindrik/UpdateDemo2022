namespace NetFrontendDayWeb.Models;

public record Session
{
    public Speaker[] Speakers { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
}
