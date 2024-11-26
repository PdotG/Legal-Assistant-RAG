using backend.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Embedding>()
                .Property(e => e.Vector)
                .HasColumnType("vector");

            modelBuilder.Entity<User>()
                .ToTable("users")
                .Property(u => u.Id)
                .HasColumnName("id_user");

            modelBuilder.Entity<Models.File>()
                .ToTable("files")
                .Property(f => f.Id)
                .HasColumnName("id_file");

            modelBuilder.Entity<Embedding>()
                .ToTable("embeddings")
                .Property(e => e.Id)
                .HasColumnName("id_embedding");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Models.File> Files { get; set; }
        public DbSet<Embedding> Embeddings { get; set; }
    }
}