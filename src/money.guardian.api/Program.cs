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


// temp solution
var serviceProvider = builder.Services.BuildServiceProvider();
var dbContext = serviceProvider.GetService<AppDbContext>();
await dbContext.Database.MigrateAsync();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGroup("api").MapEndpoints();

app.UseHttpsRedirection();

app.UseCors();

app.Run();