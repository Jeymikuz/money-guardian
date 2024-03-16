using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using money.guardian.domain.entities;
using money.guardian.infrastructure.configurations;
using money.guardian.infrastructure.events;

namespace money.guardian.infrastructure;

public sealed class AppDbContext : IdentityDbContext<User>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        RegisterEvents();
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfiguration(new ExpenseConfiguration());
        builder.ApplyConfiguration(new ExpenseGroupConfiguration());

        base.OnModelCreating(builder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetConverter>();
    }

    private void RegisterEvents()
    {
        ChangeTracker.StateChanged += BaseEntityEvents.EntityModified;
        ChangeTracker.Tracked += BaseEntityEvents.EntityCreated;
    }

    public DbSet<Expense> Expenses { get; set; }
    public DbSet<ExpenseGroup> ExpenseGroups { get; set; }
}