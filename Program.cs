using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using TestAAIbackend.Data;
using TestAAIbackend.Repositories;
using TestAAIbackend.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Obtener nombre del Key Vault desde configuración
string keyVaultName = builder.Configuration["KeyVaultName"];
var keyVaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");

// 2. Usar Managed Identity para conectarse al Key Vault
var credential = new DefaultAzureCredential();
var secretClient = new SecretClient(keyVaultUri, credential);

// 3. Obtener cadena de conexión desde el secreto
KeyVaultSecret secret = secretClient.GetSecret("SqlConnectionString");
string connectionString = secret.Value;

// 4. Configurar DbContext con la cadena obtenida (Add DbContext)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container (Controllers). DI: repository & services
builder.Services.AddControllers();
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