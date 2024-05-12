using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.MappingConfigurations;

public class UserTopicConfiguration : IEntityTypeConfiguration<UserTopic>
{
    public void Configure(EntityTypeBuilder<UserTopic> builder)
    {
        builder.ToTable("UserTopics");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");
    }
}
