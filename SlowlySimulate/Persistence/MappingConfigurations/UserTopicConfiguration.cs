using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;

public class UserTopicConfiguration : IEntityTypeConfiguration<UserTopic>
{
    public void Configure(EntityTypeBuilder<UserTopic> builder)
    {
        builder.ToTable("UserTopics");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
    }
}
