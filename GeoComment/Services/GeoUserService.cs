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


        public GeoUserService(GeoCommentsDBContext dbContext, UserManager<GeoUser> userManager, JwtManager jwtManager, ILogger<GeoUser> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtManager = jwtManager;
            _logger = logger;
        }


        public async Task<GeoUser> CreateUser(LoginCredentials credentials)
        {
            var user = new GeoUser()
            {
                UserName = credentials.UserName
            };

            try
            {
                var CreateUser =
                    await _userManager.CreateAsync(user,
                        credentials.Password);

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


        public async Task<object> Login(LoginCredentials credentials)
        {
            var thisUser = await
                _userManager.FindByNameAsync(credentials.UserName);
            if (thisUser is null) return null;

            var userSignedIn = await
                _userManager.CheckPasswordAsync(thisUser,
                    credentials.Password);

            if (userSignedIn is false) return null;

            var token = new { token = _jwtManager.GenerateJwtToken(thisUser) };

            return token;
        }

    }
}
