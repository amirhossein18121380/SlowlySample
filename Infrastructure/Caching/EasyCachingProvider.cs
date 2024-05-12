using CrossCuttingConcerns.Caching;
using EasyCaching.Core;

namespace Infrastructure.Caching;
public class EasyCachingProvider : ICache
{
    private readonly IEasyCachingProvider _cache;

    public EasyCachingProvider(IEasyCachingProvider cache)
    {
        _cache = cache;
    }

    public async Task SetAsync(string cacheKey, object model, TimeSpan timeSpan, CancellationToken cancellationToken)
    {
        await _cache.SetAsync(cacheKey, model, timeSpan, cancellationToken).ConfigureAwait(false);
    }

    public async Task<(bool HasValue, T Value)> GetAsync<T>(string cacheKey, CancellationToken cancellationToken)
    {
        // Retrieve the cached value
        var cachedEntry = await _cache.GetAsync<T>(cacheKey, cancellationToken);

        // Check if a value exists and is not null
        bool hasValue = cachedEntry != null && cachedEntry.HasValue;

        // Extract the value (or default if not found)
        T value = hasValue ? cachedEntry.Value : default;

        return (hasValue, value);
    }



    public void Remove(string key)
    {
        _cache.Remove(key);
    }
}
