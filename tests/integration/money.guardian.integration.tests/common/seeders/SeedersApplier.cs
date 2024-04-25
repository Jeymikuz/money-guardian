using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using money.guardian.infrastructure;

namespace money.guardian.integration.tests.common.seeders;

public static class SeedersApplier
{
    public static async Task Seed(AppDbContext context, IServiceScope scope)
    {
        var seeders = Assembly.GetAssembly(typeof(SeedersApplier))!.GetTypes()
            .Where(x => typeof(ISeeder).IsAssignableFrom(x) && !x.IsInterface);

        foreach (var seeder in seeders)
        {
            var seederInstance = Activator.CreateInstance(seeder) as ISeeder;
            await seederInstance!.Seed(context, scope);
        }
    }
}