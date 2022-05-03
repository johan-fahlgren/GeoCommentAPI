using GeoComment.Data;
using GeoComment.DTOs;
using GeoComment.Models;
using Microsoft.AspNetCore.Identity;

namespace GeoComment.Services
{
    public class GeoUserService
    {

        private readonly GeoCommentsDBContext _dbContext;
        private readonly UserManager<GeoUser> _userManager;
        private readonly JwtManager _jwtManager;


        public GeoUserService(GeoCommentsDBContext dbContext, UserManager<GeoUser> userManager, JwtManager jwtManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtManager = jwtManager;
        }


        public async Task<GeoUser?> FindGeoUser(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }


        public async Task<object> Login(UserData newUser)
        {
            var thisUser = await
                _userManager.FindByNameAsync(newUser.UserName);
            if (thisUser is null) return null;

            var userSignedIn = await
                _userManager.CheckPasswordAsync(thisUser,
                    newUser.Password);

            if (userSignedIn is false) return null;

            var token = new { token = _jwtManager.GenerateJwtToken(thisUser) };

            return token;
        }

    }
}
