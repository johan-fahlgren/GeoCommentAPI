using GeoComment.Data;
using GeoComment.DTOs;
using GeoComment.Models;
using GeoComment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeoComment.Controllers
{
    [Route("api/geo-comments")]
    [ApiController]
    public class GeoCommentsControllerV0_2 : ControllerBase
    {
        private readonly GeoCommentsDBContext _dbContext;
        private readonly JwtManager _jwtManager;
        private readonly UserManager<GeoUser> _userManager;
        private readonly GeoCommentService _geoCommentService;
        private readonly GeoUserService _geoUserService;


        public GeoCommentsControllerV0_2(GeoCommentsDBContext dbContext, JwtManager jwtManager, UserManager<GeoUser> userManager, GeoCommentService geoCommentService)
        {
            _dbContext = dbContext;
            _jwtManager = jwtManager;
            _userManager = userManager;
            _geoCommentService = geoCommentService;
        }



        //TODO - link comment to user
        [Authorize]
        [ApiVersion("0.2")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Comment>> AddComment(
            NewCommentV0_2 newComment)
        {
            if (string.IsNullOrWhiteSpace(newComment.Body.Author) ||
                string.IsNullOrWhiteSpace(newComment.Body.Title) ||
                string.IsNullOrWhiteSpace(newComment.Body.Message))
                return BadRequest();

            var user = HttpContext.User;
            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userName = await _geoUserService.FindGeoUser(userId);

            if (userName is null) return Unauthorized();

            var comment = new Comment()
            {
                Author = userName.UserName,
                Title = newComment.Body.Title,
                Message = newComment.Body.Message,
                Longitude = newComment.Longitude,
                Latitude = newComment.Latitude,
            };

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }




        [ApiVersion("0.2")]
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ResponseCommentV0_2>> GetComment(int id)
        {
            var comment = await _geoCommentService.FindComment(id);

            if (comment is null)
            {
                return NotFound();
            }

            ResponseCommentV0_2 thisComment;

            if (string.IsNullOrWhiteSpace(comment.Title))
            {
                var newTitle = comment.Message.Split(" ")[0];

                thisComment = new ResponseCommentV0_2()
                {
                    Id = comment.Id,
                    Latitude = comment.Latitude,
                    Longitude = comment.Longitude,

                    Body = new Body
                    {
                        Author = comment.Author,
                        Title = newTitle,
                        Message = comment.Message,
                    },

                };

                return Ok(thisComment);

            }

            thisComment = new ResponseCommentV0_2()
            {
                Id = comment.Id,
                Latitude = comment.Latitude,
                Longitude = comment.Longitude,

                Body = new Body
                {
                    Author = comment.Author,
                    Title = comment.Title,
                    Message = comment.Message,
                },

            };

            return Ok(thisComment);
        }

        /*[ApiVersion("0.2")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<Comment>> FindComments([FromQuery] decimal? minLon, [FromQuery] decimal? maxLon, [FromQuery] decimal? minLat, [FromQuery] decimal? maxLat)
        {
            if (minLon is null || maxLon is null || minLat is null ||
                maxLat is null) return BadRequest();

            var comments = await _dbContext.Comments
                .Where(c =>
                    c.Longitude >= minLon &&
                    c.Longitude <= maxLon &&
                    c.Latitude >= minLat &&
                    c.Latitude <= maxLat)
                .ToArrayAsync();

            return Ok(comments);

        }*/

    }
}
