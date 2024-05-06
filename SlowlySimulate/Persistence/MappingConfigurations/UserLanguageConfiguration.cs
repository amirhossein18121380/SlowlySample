using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;
public class UserLanguageConfiguration : IEntityTypeConfiguration<UserLanguage>
{
    public void Configure(EntityTypeBuilder<UserLanguage> builder)
    {
        builder.ToTable("UserLanguages");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
    }
}
