using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;

public class FriendConfiguration : IEntityTypeConfiguration<Friend>
{
    public void Configure(EntityTypeBuilder<Friend> builder)
    {
        builder.ToTable("Friend");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");


        builder.HasOne(f => f.RequestedTo)
            .WithMany(u => u.SentFriendRequests)
            .HasForeignKey(f => f.RequestedToId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasOne(f => f.RequestedBy)
            .WithMany(u => u.ReceivedFriendRequests)
            .HasForeignKey(f => f.RequestedById)
            .OnDelete(DeleteBehavior.NoAction);

    }
}
