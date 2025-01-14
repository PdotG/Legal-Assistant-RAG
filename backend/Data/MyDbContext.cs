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
            options.UseNpgsql(Configuration.GetConnectionString("WebApiDatabase"));
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Embedding> Embeddings { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Document> Documents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            modelBuilder.Entity<User>().ToTable("users");
            modelBuilder.Entity<Models.File>().ToTable("files");
            modelBuilder.Entity<Embedding>().ToTable("embeddings");
            modelBuilder.Entity<Client>().ToTable("clients");
            modelBuilder.Entity<Case>().ToTable("cases");
            modelBuilder.Entity<Document>().ToTable("documents");

            modelBuilder.Entity<Embedding>().Ignore(e => e.Vector);
        }
        
    }
}
