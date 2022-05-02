using GeoComment.DTOs;
using GeoComment.Models;
using GeoComment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly UserManager<GeoUser> _userManager;
        private readonly GeoUserService _userService;
        private readonly ILogger<GeoUser> _logger;

        public UserController(UserManager<GeoUser> userManager, ILogger<GeoUser> logger, GeoUserService userService)
        {
            _userManager = userManager;
            _logger = logger;
            _userService = userService;
        }


        [ApiVersion("0.2")]
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<GeoUser>> AddUser(UserData newUser)
        {

            if (string.IsNullOrWhiteSpace(newUser.UserName) || string.IsNullOrWhiteSpace(newUser.Password))
                return BadRequest();

            var user = new GeoUser()
            {
                UserName = newUser.UserName,
            };

            try
            {
                var CreateUser =
                    await _userManager.CreateAsync(user,
                        newUser.Password);

                var responseUser = new ResponseUser()
                {
                    Id = user.Id,
                    Username = user.UserName
                };


                if (CreateUser.Succeeded) return CreatedAtAction(nameof(GetUser), new { id = user.Id }, responseUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }

            return BadRequest();
        }


        [HttpGet]
        [ApiVersion("0.1")]
        [ApiVersion("0.2")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<GeoUser>> GetUser(string id)
        {
            var user = await _userService.FindGeoUser(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpPost]
        [ApiVersion("0.2")]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> LoginUser(UserData user)
        {
            var token = await _userService.Login(user);

            if (token is null) return BadRequest();

            return Ok(token);
        }

    }
}
