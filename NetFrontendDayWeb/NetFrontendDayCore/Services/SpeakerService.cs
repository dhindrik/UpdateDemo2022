using Microsoft.Extensions.Caching.Memory;
using Microsoft.JSInterop;
using NetFrontendDayCore.Services;

namespace NetFrontendDayWeb.Services;

public class SpeakerService : ISpeakerService
{

    private readonly ICacheService cacheService;
    private HttpClient client;

    public SpeakerService(ICacheService cacheService)
    {
        client = new HttpClient();
        this.cacheService = cacheService;
    }

    public Task<SpeakerInfo> GetOrganizer(string name)
    {
        var person = name switch
        {
            "daniel" => daniel,
            "jimmy" => jimmy,
            "jessica" => jessica,
            _ => throw new Exception("not found")
        };

        return Task.FromResult(person);
    }

    public async Task<SpeakerInfo> GetSpeaker(string id, int year)
    {
        var speakers = await GetSpeakers(year);

        var speaker = speakers.Single(x => x.Id == id);

        return speaker;
    }

    private string GetKey(int year)
    {
        return year switch
        {
            2021 => "",
            2022 => "",
            _ => string.Empty
        };
    }


    public async Task<List<SpeakerInfo>> GetSpeakers(int year)
    {
        try
        {
            var result = await cacheService.Get<List<SpeakerInfo>>($"{nameof(SpeakerInfo)}{year}", new List<SpeakerInfo>());

            if (result.Count > 0)
            {
                return result.OrderBy(x => x.Lastname).ToList();
            }

            var key = GetKey(year);

            var speakers = GetFromSessionize<List<Speaker>>($"https://sessionize.com/api/v2/{key}/view/Speakers");
            var sessions = GetFromSessionize<List<Group>>($"https://sessionize.com/api/v2/{key}/view/Sessions");

            await Task.WhenAll(speakers, sessions);

            var speakerInfo = speakers.Result.Select(x => new SpeakerInfo()
            {
                Id = x.Fullname.Replace(" ", "-").Replace(".",string.Empty),
                Name = x.Fullname,
                Lastname = x.Lastname,
                Tagline = x.Tagline,
                Bio = x.Bio,
                ProfilePicture = x.ProfilePicture,
                Sessions = sessions.Result[0].Sessions.Where(s => s.Speakers.Select(sp => sp.Id).Contains(x.Id)),
                Links = x.Links
            }).ToList();

            await cacheService.Save($"{nameof(SpeakerInfo)}{year}", speakerInfo);

            return speakerInfo.OrderBy(x => x.Lastname).ToList();
        }
        catch(Exception)
        {
            return new List<SpeakerInfo>();
        }
    }

    private async Task<T> GetFromSessionize<T>(string url)
    {
        var json = await client.GetStringAsync(url);

        var result = JsonSerializer.Deserialize<T>(json, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true });

        return result;
    }

    public async Task<Schedule> GetSchedule(int year)
    {
        var result = await cacheService.Get<Schedule?>($"{nameof(SpeakerInfo)}{year}", null);

        if (result != null)
        {
            return result;
        }

        var key = GetKey(year);

        var schedules = await GetFromSessionize<List<Schedule>>($"https://sessionize.com/api/v2/{key}/view/GridSmart");

        var schedule = schedules.First();

        await cacheService.Save($"{nameof(SpeakerInfo)}{year}", schedule);

        return schedule;
    }

    public Task<List<Recording>> GetRecordings(string key)
    {
        var recordings = new List<Recording>()
        {
            new Recording()
            {
                Speaker = "Jeffrey T. Fritz",
                Session = "Blazor for the Web Forms Developer",
                YouTubeUrl = "https://www.youtube.com/embed/HwefEPGe6ho"
            },
            new Recording()
            {
                Speaker = "Nico Vermeir",
                Session = "The future of .NET desktop development",
                YouTubeUrl = "https://www.youtube.com/embed/k-KnVvTH_nw"
            },
            new Recording()
            {
                Speaker = "Stacy Cashmore",
                Session = "Deploying Client Side Blazor to a Static Web App",
                YouTubeUrl = "https://www.youtube.com/embed/1VzG0PIcfq8"
            },
            new Recording()
            {
                Speaker = "Mark Allibone",
                Session = "",
                YouTubeUrl = "https://www.youtube.com/embed/Rcen16onPKo"
            },
            new Recording()
            {
                Speaker = "Sam Basu",
                Session = "Mobile & Desktop Apps with Blazor!",
                YouTubeUrl = "https://www.youtube.com/embed/4P8RyNN3Zzk"
            },
            new Recording()
            {
                Speaker = "Steven Bilogan",
                Session = "Create Applications with WinUI – Everywhere!",
                YouTubeUrl = "https://www.youtube.com/embed/tmsqPttFLK4"
            },
            new Recording()
            {
                Speaker = "Georgia Nelson",
                Session = "Winforms to Blazor: A GIFBot Postmortem",
                YouTubeUrl = "https://www.youtube.com/embed/el5bCT14Lc0"
            },
            new Recording()
            {
                Speaker = "David Ortinau",
                Session = "A .NET MAUI Progress Report",
                YouTubeUrl = "https://www.youtube.com/embed/RnyZZKjdUxk"
            }
        };

        return Task.FromResult(recordings);
    }

    private SpeakerInfo jessica = new SpeakerInfo()
    {
        Name = "Jessica Engström",
        ProfilePicture = "jessica.jpeg",
        Tagline = "Azm Dev",
        Bio = "Being a geek shows in all parts of her life, whether it be organizing hackathons, running a user group and a podcast with her husband, game nights (retro or VR/MR) with friends, just catching the latest superhero movie or speaking internationally at conferences. Her favorite topics are UX / UI, Mixed reality and other futuristic tech.She's a Windows Development MVP. Together with her husband she runs a company called \"AZM dev\" which is focused on HoloLens, Windows development, UX and teaching the same.",
        Sessions = new List<Session>(),
        Links = new List<Link>()
        {
            new Link(){Title ="Twitter", Url = "https://twitter.com/EngstromJess"},
            new Link() {Title ="LinkedIn", Url = "https://www.linkedin.com/in/jessicaengstrom/"},
            new Link() {Title="Blog", Url ="http://www.catoholic.se/"},
            new Link() {Title="Company", Url="http://www.azm.se/"},
            new Link() {Title="Podcast", Url="http://www.codingafterwork.se/"}
        }
    };

    private SpeakerInfo jimmy = new SpeakerInfo()
    {
        Name = "Jimmy Engström",
        ProfilePicture = "jimmy.jpeg",
        Tagline = "Windows Development MVP",
        Bio = "Ever since Jimmy got his first ZX Spectrum at the age of 7 he hasn’t stopped programming. During the day he is an .NET developer and he does all the fun stuff during his spare time. Together with his wife he also runs a company ”azm dev” which is focused on HoloLens and windows development. He is really passionate about Windows development, HoloLens or well you could say the .NET platform really. He and his wife (Jessica) also runs a code intensive user group (Coding After Work) that focuses on helping participants with code and design problems, and a podcast with the same name. Back in 2011 he received the ”geek of the year” award, a title well deserved. He is certainly living up to the title with everything from developing and gadgets to superheroes and cosplaying at the sci-fi fair. He speaks at various types of events including NDC, Devsum, hackathons, Swetugg and TechDays which led to him becoming a Microsoft MVP in Windows Development.",
        Sessions = new List<Session>(),
        Links = new List<Link>()
        {
            new Link(){Title ="Twitter", Url = "https://twitter.com/EngstromJimmy"},
            new Link() {Title ="LinkedIn", Url = "https://www.linkedin.com/in/apeoholic/"},
            new Link() {Title="Blog", Url ="http://www.engstromjimmy.se/"},
            new Link() {Title="Company", Url="http://www.azm.se/"},
            new Link() {Title="Podcast", Url="http://www.codingafterwork.se/"}
        }
    };

    private SpeakerInfo daniel = new SpeakerInfo()
    {
        Name = "Daniel Hindrikes",
        ProfilePicture = "daniel.jpg",
        Tagline = "App Innovation Architect",
        Bio = "Daniel is a developer and architect that has his passion for developing mobile apps powered by the cloud. Daniel has been developed iOS- and Android with Xamarin since the early days of Xamarin. But he started to develop mobile apps even before that, mostly on the Android- and Windows platforms.Daniel enjoying to share his knowledge by speaking, blogging or recording the podcast, The Code Behind. Daniel is also co - author of the book, Xamarin.Forms projects and a Microsoft MVP in Developer Technologies.Daniel working for tretton37 in Sweden and he has experience for working with both local - and global customers.",
        Sessions = new List<Session>(),
        Links = new List<Link>()
        {
            new Link(){Title ="Twitter", Url = "https://twitter.com/hindrikes"},
            new Link() {Title ="LinkedIn", Url = "https://www.linkedin.com/in/daniel-hindrikes/"},
            new Link() {Title="Blog", Url ="http://danielhindrikes.se/"},
            new Link() {Title="Company", Url="http://tretton37.com"},
        }
    };
}
