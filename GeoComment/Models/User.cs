using Microsoft.AspNetCore.Identity;

namespace GeoComment.Models
{
    public class User : IdentityUser
    {
        public string Password { get; set; }

        public ICollection<Comment>? Comments { get; set; }
    }
}
