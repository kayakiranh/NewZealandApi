using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Data
{
    [Serializable]
    public class NzwDbContext : DbContext
    {
        //add-migration "Creating_Auth_Database" -Context "NzwDbContext"
        //update-database -Context "NzwDbContext"

        public NzwDbContext(DbContextOptions<NzwDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<WalkRoute> WalkRoutes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Difficulty>(entity =>
            {
                entity.Property<Guid>("Id").ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsClustered(false);
            });
            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property<Guid>("Id").ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsClustered(false);
                entity.HasIndex(e => e.Code).IsClustered(false);
            });
            modelBuilder.Entity<WalkRoute>(entity =>
            {
                entity.Property<Guid>("Id").ValueGeneratedOnAdd();
                entity.HasIndex(e => e.Name).IsClustered(false);
            });
        }
    }
}