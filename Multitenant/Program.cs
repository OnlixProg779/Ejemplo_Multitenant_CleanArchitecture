using Multitenant.Infraestructure;
using Multitenant.Application;
using Microsoft.AspNetCore.Identity;
using Multitenant.Infraestructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infraestructure
builder.Services.ConfigureInfraestructureServices(builder.Configuration);
builder.Services.ConfigureBussinesServices(builder.Configuration);
builder.Services.ConfigureIdentityServices(builder.Configuration);
builder.Services.AddExtendJwtServices(builder.Configuration);

// application
builder.Services.AddExtendApplicationServices();

builder.Services.AddRolesServices();

var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider;
    var loggerFactory = service.GetRequiredService<ILoggerFactory>();

    try
    {
        var context = service.GetRequiredService<IdentityOrganizationDbContext>();
        await context.Database.MigrateAsync();

        var userManager = service.GetRequiredService<UserManager<IdentityUser>>();
        var RoleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
        await IdentityOrganizationSeedData.SeedAsync(userManager, RoleManager, loggerFactory);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "Error en migration");
    }
}

await ExtendApplicationServiceRegistration.LoadRolesAsync(app);

app.Run();
