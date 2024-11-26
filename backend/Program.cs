using backend.Data;
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

try
{
    using var connection = new NpgsqlConnection(connectionString);
    connection.Open();
    Console.WriteLine("Conexión exitosa con la base de datos.");
}
catch (Exception ex)
{
    Console.WriteLine($"Error al conectar con la base de datos: {ex.Message}");
}
