using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Models;

namespace Persistence.Data;

public class AccountMap : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.HasKey(ps => ps.Id);
        builder.Property(ps => ps.AccountNumber).IsRequired();
        builder.Property(ps => ps.UserId).IsRequired();
        builder
            .HasOne(ps => ps.User)
            .WithMany(u => u.Accounts)
            .HasForeignKey(ps => ps.UserId);
    }
}