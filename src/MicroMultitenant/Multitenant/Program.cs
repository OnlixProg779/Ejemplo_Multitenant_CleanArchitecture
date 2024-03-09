using Multitenant.Infraestructure;
using Multitenant.Application;
using Multitenant.Middlewares;
using Base.Infraestructure;
using Base.Application;
using DsAlpha.RedisStream;
using Hangfire;
using Hangfire.Dashboard;
using Multitenant.Infraestructure.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infraestructure
builder.Services.ConfigureInfraestructureServices(builder.Configuration);
builder.Services.ConfigureMultitenantBussinesServices(builder.Configuration);
builder.Services.ConfigureBaseInfraestructureServices(builder.Configuration);
builder.Services.AddExtendJwtServices(builder.Configuration);

// application
builder.Services.AddExtendApplicationServices();
builder.Services.AddMultitenantApplicationServices();
builder.Services.AddRolesServices();
builder.Services.ConfigureRedisServices(builder.Configuration);


builder.Services.ConfigureHangfireClienteServices(builder.Configuration);
builder.Services.ConfigureHangfireServerServices<InitOrganizationJob>();
builder.Services.ConfigurePublishRedisHangfireServices();

builder.Services.AddHttpContextAccessor();

builder.Services.AddCors(options => 
{
    options.AddPolicy("CorsPolicy", builder => builder
     .AllowAnyOrigin() 
     .AllowAnyMethod()
     .AllowAnyHeader()
    );
});


var app = builder.Build();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("CorsPolicy");

app.UseAuthentication();

app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    // Configuración de autorización
    Authorization = new[] { new HangfireAuthorizationFilter() }
});

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<TenantMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();


public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Implementa tu lógica de autorización aquí
        // Ejemplo: permite el acceso solo en desarrollo
        var httpContext = context.GetHttpContext();
        return httpContext.User.Identity.IsAuthenticated || Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
    }
}