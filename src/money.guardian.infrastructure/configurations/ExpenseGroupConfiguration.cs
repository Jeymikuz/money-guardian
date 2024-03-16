﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using money.guardian.domain.entities;

namespace money.guardian.infrastructure.configurations;

public class ExpenseGroupConfiguration : IEntityTypeConfiguration<ExpenseGroup>
{
    public void Configure(EntityTypeBuilder<ExpenseGroup> builder)
    {
        builder.HasOne(x => x.User)
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .IsRequired();
        builder.HasIndex(x => x.UserId);
    }
}