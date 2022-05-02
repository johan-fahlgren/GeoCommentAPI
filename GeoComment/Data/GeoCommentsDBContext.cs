using GeoComment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Data

{
    public class GeoCommentsDBContext : IdentityDbContext<IdentityUser>
    {
        public GeoCommentsDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Comment>? Comments { get; set; }
        public DbSet<User>? Users { get; set; }

    }
}
