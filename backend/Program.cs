using backend.Data;
using backend.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Verificar la carga de la configuración
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var connectionString = builder.Configuration.GetConnectionString("WebApiDatabase");

if (string.IsNullOrEmpty(connectionString))
{
    Console.WriteLine("Error: No se encontró la cadena de conexión.");
    return;
}
else
{
    Console.WriteLine($"Cadena de conexión encontrada: {connectionString}");
}

// Registrar servicios
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<IEmbeddingRepository, EmbeddingRepository>();

builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Build the web application
var app = builder.Build();

// Configure the HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

// Start the application
app.Run("http://localhost:3000");
