using Microsoft.Extensions.DependencyInjection;
using money.guardian.infrastructure;

namespace money.guardian.integration.tests.common;

public interface ISeeder
{
    Task Seed(AppDbContext context, IServiceScope scope);
}