using Microsoft.EntityFrameworkCore;
using System.Text;
using backend.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        // Entry point of the application
        var builder = WebApplication.CreateBuilder(args);

        // Adding services to the controller
        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();

        // Database Services
        builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("WebApiDatabase")));

        // Build the web application
        var app = builder.Build();

        // Configure the HTTP Pipeline
        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.MapControllers();

        // Start the application
        app.Run();
    }
}