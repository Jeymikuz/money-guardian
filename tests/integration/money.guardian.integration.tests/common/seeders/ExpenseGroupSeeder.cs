using Microsoft.Extensions.DependencyInjection;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.integration.tests.common.seeders;

public class ExpenseGroupSeeder : ISeeder
{
    public async Task Seed(AppDbContext context, IServiceScope scope)
    {
        var expenseGroup = new ExpenseGroup()
        {
            Name = TestDataConstants.ExpenseGroup.Name,
            Icon = TestDataConstants.ExpenseGroup.Icon,
            UserId = TestDataConstants.User.Id1
        };

        await context.ExpenseGroups.AddAsync(expenseGroup);
        await context.SaveChangesAsync();
    }
}