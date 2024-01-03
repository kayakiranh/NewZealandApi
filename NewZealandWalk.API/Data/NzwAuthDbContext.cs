using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewZealandWalk.API.Data
{
    public class NzwAuthDbContext : IdentityDbContext
    {
        public NzwAuthDbContext(DbContextOptions<NzwAuthDbContext> options) : base(options)
        {

        }
    }
}