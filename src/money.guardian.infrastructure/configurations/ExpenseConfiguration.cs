using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using money.guardian.domain.entities;

namespace money.guardian.infrastructure.configurations;

public class ExpenseConfiguration : IEntityTypeConfiguration<Expense>
{
    public void Configure(EntityTypeBuilder<Expense> builder)
    {
        builder.HasOne(x => x.Group).WithMany().HasForeignKey(x => x.GroupId).IsRequired(false);
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
        builder.HasIndex(x => x.UserId);
    }
}