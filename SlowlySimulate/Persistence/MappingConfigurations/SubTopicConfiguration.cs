using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SlowlySimulate.Domain.Models;

namespace SlowlySimulate.Persistence.MappingConfigurations;

public class SubTopicConfiguration : IEntityTypeConfiguration<SubTopic>
{
    public void Configure(EntityTypeBuilder<SubTopic> builder)
    {
        builder.ToTable("SubTopics");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
    }
}
