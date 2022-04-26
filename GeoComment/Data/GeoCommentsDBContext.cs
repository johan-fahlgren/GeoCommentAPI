using GeoComment.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Data

{
    public class GeoCommentsDBContext : DbContext
    {
        public GeoCommentsDBContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Comment>? Comments { get; set; }

    }
}
