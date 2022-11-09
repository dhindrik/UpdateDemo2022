namespace NetFrontendDayWeb.Services;

public interface ISpeakerService
{
    Task<List<SpeakerInfo>> GetSpeakers(int year);
    Task<SpeakerInfo> GetSpeaker(string id, int year);
    Task<SpeakerInfo> GetOrganizer(string name);
    Task<Schedule> GetSchedule(int year);
    Task<List<Recording>> GetRecordings(string key);
}
