namespace NetFrontendDayWeb.Models;

public record Schedule
{
    public DateTimeOffset? Date { get; init; }
    public List<ScheduleRoom> Rooms { get; init; }
    public List<TimeSlot> TimeSlots { get; init; }
}

public record ScheduleRoom
{
    public long Id { get; init; }
    public string Name { get; init; }
    public List<ScheduleSession> Sessions { get; init; }
    public bool HasOnlyPlenumSessions { get; init; }
}

public record ScheduleSession
{
    public string Id { get; init; }
    public string Title { get; init; }
    public string Description { get; init; }
    public DateTime? StartsAt { get; init; }
    public DateTime? EndsAt { get; init; }
    public bool IsServiceSession { get; init; }
    public bool IsPlenumSession { get; init; }
    public List<ScheduleSpeaker> Speakers { get; init; }
    public List<object> Categories { get; init; }
    public long RoomId { get; init; }
    public string Room { get; init; }
}

public record ScheduleSpeaker
{
    public string InternalId => Name.Replace(" ", "-").Replace(".", string.Empty);
    public Guid Id { get; init; }
    public string Name { get; init; }
}

public record TimeSlot
{
    public string SlotStart { get; init; }
    public List<TimeSlotRoom> Rooms { get; init; }
}

public record TimeSlotRoom
{
    public long Id { get; init; }
    public string Name { get; init; }
    public Session Session { get; init; }
    public long Index { get; init; }
}
