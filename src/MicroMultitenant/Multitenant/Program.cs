using Multitenant.Infraestructure;
using Multitenant.Application;
using Multitenant.Middlewares;
using Base.Infraestructure;
using Base.Application;

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

app.UseAuthorization();

app.MapControllers();

app.UseMiddleware<TenantMiddleware>();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();
