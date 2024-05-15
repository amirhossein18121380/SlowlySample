using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.MappingConfigurations;

public class LetterConfiguration : IEntityTypeConfiguration<Letter>
{
    public void Configure(EntityTypeBuilder<Letter> builder)
    {
        builder.ToTable("Letters");
        builder.Property(x => x.Id).HasDefaultValueSql("newsequentialid()");

        builder
            .HasOne(l => l.Sender)
            .WithMany(u => u.SentLetters)
            .HasForeignKey(l => l.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(l => l.Recipient)
            .WithMany(u => u.ReceivedLetters)
            .HasForeignKey(l => l.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
