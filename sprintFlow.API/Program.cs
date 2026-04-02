using Microsoft.AspNetCore.Identity;
using sprintFlow.Domain.Entities;
using sprintFlow.Infrastructure.Extensions;
using sprintFlow.Infrastructure.Persistence;
using sprintFlow.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

//builder.Services
//    .AddIdentityApiEndpoints<User>()
//    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddControllers();


var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

//app.MapIdentityApi<User>();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await DatabaseSeeder.SeedAsync(services);
}


app.Run();