namespace Infrastructure.Caching;
public class CacheResult<T>
{
    public bool HasValue { get; set; }
    public T Value { get; set; }
}