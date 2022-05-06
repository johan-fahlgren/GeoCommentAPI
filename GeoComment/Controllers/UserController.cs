using GeoComment.DTOs;
using GeoComment.Models;
using GeoComment.Services;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GeoUserService _userService;


        public UserController(GeoUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Add user to database
        /// </summary>
        /// <param name="newUser">DTO with username and password</param>
        /// <returns>Returns user data based on ResponseUser DTO</returns>
        [ApiVersion("0.2")]
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<GeoUser>> AddUser(LoginCredentials newUser)
        {

            if (string.IsNullOrWhiteSpace(newUser.UserName) || string.IsNullOrWhiteSpace(newUser.Password))
                return BadRequest();

            var userCreated = await _userService.CreateUser(newUser);

            if (userCreated is null) return BadRequest();

            var responseUser = new ResponseUser()
            {
                Id = userCreated.Id,
                Username = userCreated.UserName
            };

            return CreatedAtAction(nameof(GetUser), new { id = userCreated.Id }, responseUser);
        }

        /// <summary>
        /// Get User by Guid
        /// </summary>
        /// <param name="id">User guid string</param>
        /// <returns>Returns user data based on ResponseUser DTO</returns>
        [HttpGet]
        [ApiVersion("0.1")]
        [ApiVersion("0.2")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<ResponseUser>> GetUser(string id)
        {
            var user = await _userService.FindGeoUser(id);

            if (user is null)
            {
                return NotFound();
            }

            var responseUser = new ResponseUser()
            {
                Id = user.Id,
                Username = user.UserName
            };


            return Ok(responseUser);
        }

        /// <summary>
        /// Login user using username and password
        /// </summary>
        /// <param name="user">DTO with username and password</param>
        /// <returns>Returns JWT</returns>
        [HttpPost]
        [ApiVersion("0.2")]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<object>> LoginUser(LoginCredentials user)
        {
            var token = await _userService.Login(user);

            if (token is null) return BadRequest();

            return Ok(token);
        }

    }
}
