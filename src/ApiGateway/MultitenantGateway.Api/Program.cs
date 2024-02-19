using MultitenantGateway.Api.HelperHandler;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n personalizada para ignorar la validaci�n de certificados SSL/TLS
builder.Services.AddHttpClient("OcelotClientHandler").ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback = (message, certificate, chain, sslPolicyErrors) => true
});

// Add services to the container.
builder.Services.AddOcelot()
                .AddDelegatingHandler<CustomDelegatingHandler>(); // Aseg�rate de tener una clase CustomDelegatingHandler que implemente DelegatingHandler


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile($"ocelot.json", optional: false, reloadOnChange: true);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// Si realmente necesitas MapControllers por alguna raz�n espec�fica, aseg�rate de que est� justificado
// app.MapControllers();
await app.UseOcelot();
//app.UseMiddleware<TestMiddleware>();

app.Run();
