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
builder.Services.AddTransient<EmbeddingsHelper>();
builder.Services.AddSingleton(sp => new OpenAIClient(builder.Configuration["OpenAI:OPENAI_API_KEY"]));

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddControllers(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];
var jwtAudience = builder.Configuration["Jwt:Audience"];

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

// Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseCors("AllowAll");
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

app.Run("http://localhost:3000");
