using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewZealandWalk.API.Data
{
    [Serializable]
    public class NzwAuthDbContext : IdentityDbContext
    {
        //add-migration "Creating_Auth_Database" -Context "NzwAuthDbContext"
        //update-database -Context "NzwAuthDbContext"

        public NzwAuthDbContext(DbContextOptions<NzwAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            List<IdentityRole> roles = new List<IdentityRole> {
                new IdentityRole{ Id = "37e6c07a-48c4-422a-8943-07f295372e6a", ConcurrencyStamp = "37e6c07a-48c4-422a-8943-07f295372e6a", Name = "Reader", NormalizedName = "Reader" },
                new IdentityRole{ Id = "4c7c83f1-f984-4b31-ac29-6b23d2cb71ae", ConcurrencyStamp = "4c7c83f1-f984-4b31-ac29-6b23d2cb71ae", Name = "Writer", NormalizedName = "Writer" },
            };

            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}