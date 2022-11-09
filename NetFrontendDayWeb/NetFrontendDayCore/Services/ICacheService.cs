namespace NetFrontendDayCore.Services;

	public interface ICacheService
	{
		Task<T> Get<T>(string key, T defaultValue);
		Task Save<T>(string key, T value);
	}

