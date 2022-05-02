using GeoComment.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Data

{
    public class GeoCommentsDBContext : IdentityDbContext<User>
    {
        public GeoCommentsDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Comment>? Comments { get; set; }

    }
}
