using System.Text;
using backend.Data;
using backend.Data.Repositories;
using backend.Filters;
using backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OpenAI;

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
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<EmbeddingRepository>();
builder.Services.AddScoped<IEmbeddingRepository, EmbeddingRepository>();
builder.Services.AddScoped<FileRepository>();
builder.Services.AddScoped<IFileRepository, FileRepository>();
builder.Services.AddScoped<ClientRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<CaseRepository>();
builder.Services.AddScoped<ICaseRepository, CaseRepository>();
builder.Services.AddScoped<DocumentRepository>();
builder.Services.AddScoped<IDocumentRepository, DocumentRepository>();

builder.Services.AddTransient<PdfHelper>();
builder.Services.AddSingleton(sp => new OpenAIClient(builder.Configuration["OpenAI:OPENAI_API_KEY"]));

builder.Services.AddAutoMapper(typeof(Program));

// Registrar filtros globales
// builder.Services.AddControllers(options =>
// {
//     options.Filters.Add<GlobalExceptionFilter>(); // Registro del filtro personalizado
// });
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Configuring JWT Authentication
var jwtKey = builder.Configuration["Jwt:Key"]; // JWT Key
var jwtIssuer = builder.Configuration["Jwt:Issuer"]; // JWT Issuer
var jwtAudience = builder.Configuration["Jwt:Audience"]; // JWT Audience

// Check if the JWT config values are missing
if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration values are missing");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Agregar Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// Build the web application
var app = builder.Build();

// Configure the HTTP Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

//app.UseHttpsRedirection();

app.UseRouting();

// Agregar autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

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

// Start the application
app.Run("http://localhost:3000");
