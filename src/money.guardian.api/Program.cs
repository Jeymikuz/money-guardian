using Microsoft.EntityFrameworkCore;
using money.guardian.api.endpoints;
using money.guardian.api.extensions;
using money.guardian.infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddServices(builder.Configuration);

builder.Services.AddCors(x =>
    x.AddDefaultPolicy(y =>
        y.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()));

var app = builder.Build();

//  temp solution
var serviceProvider = app.Services.CreateScope();
var dbContext = serviceProvider.ServiceProvider.GetRequiredService<AppDbContext>();
await dbContext.Database.MigrateAsync();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("api").MapEndpoints();

app.UseHttpsRedirection();

app.UseCors();

app.Run();

public partial class Program;