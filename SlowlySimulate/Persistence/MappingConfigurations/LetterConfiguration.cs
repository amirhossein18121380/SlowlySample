using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;

public class LetterConfiguration : IEntityTypeConfiguration<Letter>
{
    public void Configure(EntityTypeBuilder<Letter> builder)
    {
        builder.ToTable("Letters");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");

    }
}
