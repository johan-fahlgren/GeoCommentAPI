using Microsoft.AspNetCore.Identity;

namespace GeoComment.Models
{
    public class GeoUser : IdentityUser
    {
        public ICollection<Comment>? Comments { get; set; }
    }
}
