using GeoComment.Data;
using GeoComment.DTOs;
using GeoComment.Models;
using GeoComment.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("api/geo-comments")]
    [ApiController]
    public class GeoCommentsControllerV0 : ControllerBase
    {
        private readonly GeoCommentsDBContext _dbContext;
        private readonly JwtManager _jwtManager;
        private readonly UserManager<GeoUser> _userManager;
        private readonly GeoCommentService _geoCommentService;


        public GeoCommentsControllerV0(GeoCommentsDBContext dbContext, JwtManager jwtManager, UserManager<GeoUser> userManager, GeoCommentService geoCommentService)
        {
            _dbContext = dbContext;
            _jwtManager = jwtManager;
            _userManager = userManager;
            _geoCommentService = geoCommentService;
        }



        //TODO - link comment to user
        [ApiVersion("0.2")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Comment>> AddComment(
            NewComment newComment)
        {
            if (string.IsNullOrWhiteSpace(newComment.Author) || string.IsNullOrWhiteSpace(newComment.Message))
                return BadRequest();

            var comment = new Comment()
            {
                Author = newComment.Author,
                Message = newComment.Message,
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
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            var comment = await _geoCommentService.FindComment(id);

            if (comment is null)
            {
                return NotFound();
            }

            return Ok(comment);
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
