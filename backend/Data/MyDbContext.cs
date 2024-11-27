using backend.Models;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace backend.Data
{
    public class MyDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public MyDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Configuración de la conexión a PostgreSQL
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        // DbSet para las demás entidades
        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Embedding> Embeddings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración de las tablas
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Models.File>().ToTable("files");
            modelBuilder.Entity<Embedding>().ToTable("embeddings");

            // Ignorar la propiedad Vector para que no sea mapeada automáticamente
            modelBuilder.Entity<Embedding>().Ignore(e => e.Vector);
        }
        
    }
}
