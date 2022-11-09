using System;
using NetFrontendDayCore.Services;
using NetFrontendDayWeb.Models;

namespace NetFrontendDayWeb.Services
{
	public class MemoryCacheService : ICacheService
	{
        private readonly IMemoryCache memoryCache;

        public MemoryCacheService(IMemoryCache memoryCache)
		{
            this.memoryCache = memoryCache;
        }

        public Task<T> Get<T>(string key, T defaultValue)
        {
            if (memoryCache.TryGetValue(key, out T result))
            {
                return Task.FromResult(result);
            }

            return Task.FromResult(defaultValue);

        }

        public Task Save<T>(string key, T value)
        {
            memoryCache.Set(key, value, DateTimeOffset.Now.AddHours(1));

            return Task.CompletedTask;
        }
    }
}

