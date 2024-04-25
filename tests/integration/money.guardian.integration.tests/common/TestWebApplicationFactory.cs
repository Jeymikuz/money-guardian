using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using money.guardian.infrastructure;
using Npgsql;
using Respawn;
using Testcontainers.PostgreSql;

namespace money.guardian.integration.tests.common;

public class TestWebApplicationFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:16-alpine")
        .WithDatabase("mgDB")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private DbConnection _dbConnection = default!;
    protected Respawner _respawner = default!;

    public HttpClient HttpClient { get; private set; } = default!;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            var descriptor = services.SingleOrDefault(x => x.ServiceType == typeof(DbContextOptions<AppDbContext>));

            if (descriptor is not null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<AppDbContext>(options => { options.UseNpgsql(_dbContainer.GetConnectionString()); });

            var serviceProvider = services.BuildServiceProvider();
            var appDbContext = serviceProvider.GetService<AppDbContext>();
            appDbContext.Database.Migrate();
        });
    }

    public async Task ResetDatabaseAsync()
    {
        await _respawner.ResetAsync(_dbConnection);
    }

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _dbConnection = new NpgsqlConnection(_dbContainer.GetConnectionString());
        HttpClient = CreateClient();
        await InitRespawner();
    }

    private async Task InitRespawner()
    {
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(_dbConnection, new RespawnerOptions()
        {
            DbAdapter = DbAdapter.Postgres,
        });
    }

    public new Task DisposeAsync()
    {
        return _dbContainer.StopAsync();
    }
}