using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;
public class SlowlyUserConfiguration : IEntityTypeConfiguration<SlowlyUser>
{
    public void Configure(EntityTypeBuilder<SlowlyUser> builder)
    {
        builder.ToTable("SlowlyUsers");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");

    }
}
