using GeoComment.Data;
using GeoComment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly GeoCommentsDBContext _dbContext;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<User> _logger;

        public UserController(GeoCommentsDBContext dbContext, UserManager<User> userManager, ILogger<User> logger)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _logger = logger;
        }


        [ApiVersion("0.2")]
        [Route("register")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<User>> AddUser(NewUser newUser)
        {

            if (string.IsNullOrWhiteSpace(newUser.UserName) || string.IsNullOrWhiteSpace(newUser.Password))
                return BadRequest();

            var user = new User()
            {
                UserName = newUser.UserName,
            };

            try
            {
                var CreateUser =
                    await _userManager.CreateAsync(user,
                        newUser.Password);
                if (CreateUser.Succeeded) return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception ex)
            {
                _logger.LogError("failed to create user",
                    ex.ToString());

            }

            return BadRequest();
        }

        public class NewUser
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }

        }


        [HttpGet]
        [ApiVersion("0.1")]
        [ApiVersion("0.2")]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            User? user = await _dbContext.Users.FindAsync(id);

            if (user is null)
            {
                return NotFound();
            }

            return Ok(user);
        }

    }
}
