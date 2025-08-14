using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using TestAAIbackend.Data;
using TestAAIbackend.Repositories;
using TestAAIbackend.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar conexión con Azure Key Vault
string keyVaultUrl = builder.Configuration["KeyVaultUrl"]; // ponlo en appsettings.json o en Azure App Settings

if (string.IsNullOrEmpty(keyVaultUrl))
{
    throw new InvalidOperationException("KeyVaultUrl no está configurado en appsettings.json o en Azure App Settings.");
}

// 2. Crear cliente de Key Vault usando Managed Identity (RBAC)
var client = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());

// Obtiene la cadena de conexión desde Key Vault de manera asíncrona
KeyVaultSecret secret = await client.GetSecretAsync("DbConnectionString");
string connectionString = secret.Value;

// Add services to the container.

builder.Services.AddControllers();

// Add DbContext con Managed Identity
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Inyección de dependencias: repository & services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TestAAI Backend API",
        Version = "v1",
        Description = "API para manejar usuarios en Azure SQL",
        Contact = new OpenApiContact
        {
            Name = "Juan Palomo",
            Email = "tuemail@dominio.com"
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline. (Configure middleware)
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger(); // Genera el JSON en /swagger/v1/swagger.json
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("../swagger/v1/swagger.json", "TestAAI Backend API v1");
        c.RoutePrefix = "swagger"; // Esto mantiene la URL /swagger
    });
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
