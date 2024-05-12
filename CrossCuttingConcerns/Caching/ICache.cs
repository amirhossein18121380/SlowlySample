
namespace CrossCuttingConcerns.Caching;

public interface ICache
{
    Task SetAsync(string cacheKey, object model, TimeSpan timeSpan, CancellationToken cancellationToken);
    Task<(bool HasValue, T Value)> GetAsync<T>(string cacheKey, CancellationToken cancellationToken);
}