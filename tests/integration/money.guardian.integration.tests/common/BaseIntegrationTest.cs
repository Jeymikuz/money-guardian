using Microsoft.Extensions.DependencyInjection;
using money.guardian.infrastructure;

namespace money.guardian.integration.tests.common;

public class BaseIntegrationTest : IAsyncLifetime
{
    protected readonly IServiceScope Scope;
    protected readonly HttpClient HttpClient;
    protected readonly AppDbContext DbContext;
    private readonly Func<Task> _resetDatabase;

    protected BaseIntegrationTest(TestWebApplicationFactory factory)
    {
        Scope = factory.Services.CreateScope();
        HttpClient = factory.CreateClient();
        _resetDatabase = factory.ResetDatabaseAsync;
        DbContext = Scope.ServiceProvider.GetService<AppDbContext>()!;
    }

    private Task SeedData()
    {
        return Task.CompletedTask;
    }

    public virtual async Task InitializeAsync() => await SeedData();

    public virtual async Task DisposeAsync() => await _resetDatabase();
}