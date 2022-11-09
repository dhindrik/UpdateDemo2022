using System.Text.Json;
using NetFrontendDayCore.Services;

namespace NetFrontendDayApp.Services;

public class FileCacheService : ICacheService
{
    public async Task<T> Get<T>(string key, T defaultValue)
    {
        var name = GetName(key);

        var path = Path.Combine(FileSystem.CacheDirectory, name);

        if (File.Exists(path))
        {
            var json = await File.ReadAllTextAsync(path);

            return JsonSerializer.Deserialize<T>(json);
        }

        return defaultValue;
    }

    public async Task Save<T>(string key, T value)
    {
        var name = GetName(key);

        var path = Path.Combine(FileSystem.CacheDirectory, name);

        var json = JsonSerializer.Serialize(value);

        await File.WriteAllTextAsync(path, json);
    }

    private string GetName(string key)
    {
        var name = $"{key.Replace(" ", "_")}.json";

        return name;
    }
}

