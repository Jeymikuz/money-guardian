using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using money.guardian.domain.entities;
using money.guardian.infrastructure;

namespace money.guardian.integration.tests.common.seeders;

public class UserSeeder : ISeeder
{
    public async Task Seed(AppDbContext context, IServiceScope scope)
    {
        var userManager = scope.ServiceProvider.GetService<UserManager<User>>();

        foreach (var data in FakeUsersEmails())
        {
            var testUser = new User { Id = data.Item2, Email = data.Item1, UserName = data.Item1 };

            await userManager.CreateAsync(testUser, TestDataConstants.User.Password);
        }
    }

    private static IEnumerable<(string, string)> FakeUsersEmails() => new[]
    {
        (TestDataConstants.User.Email1, TestDataConstants.User.Id1),
        (TestDataConstants.User.Email2, TestDataConstants.User.Id2)
    };
}