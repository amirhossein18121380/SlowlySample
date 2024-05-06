using System.ComponentModel.DataAnnotations;

namespace SlowlySimulate.Domain.Models;
public interface IHasKey<T>
{
    public T Id { get; set; }
}
public interface ITrackable
{
    public byte[] RowVersion { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }
}
public interface IAggregateRoot
{
}


public abstract class Entity<TKey> : IHasKey<TKey>, ITrackable
{
    public TKey Id { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }

    public DateTimeOffset CreatedDateTime { get; set; }

    public DateTimeOffset? UpdatedDateTime { get; set; }
}
