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
        private readonly ILogger<GeoUser> _logger;


        public GeoUserService(GeoCommentsDBContext dbContext, UserManager<GeoUser> userManager, JwtManager jwtManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtManager = jwtManager;
        }


        public async Task<GeoUser> CreateUser(UserData newUser)
        {
            var user = new GeoUser()
            {
                UserName = newUser.UserName
            };

            try
            {
                var CreateUser =
                    await _userManager.CreateAsync(user,
                        newUser.Password);

                if (CreateUser.Succeeded) return user;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return null;

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
