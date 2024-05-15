using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.MappingConfigurations;

public class FriendShipConfiguration : IEntityTypeConfiguration<FriendShip>
{
    public void Configure(EntityTypeBuilder<FriendShip> builder)
    {
        builder.ToTable("Friend");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");

        builder
            .HasOne(f => f.RequestedByUser)
            .WithMany(u => u.FriendShips)
            .HasForeignKey(f => f.RequestedById)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(f => f.RequestedToUser)
            .WithMany()
            .HasForeignKey(f => f.RequestedToId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
