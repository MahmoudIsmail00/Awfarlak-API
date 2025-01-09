namespace Application.Services.CacheService
{
    public interface ICacheService
    {
        Task SetCaacheResponseAsync(string cacheKey, object response, TimeSpan timeToLive);
        Task<string> GetCaacheResponseAsync(string cacheKey);
    }
}
