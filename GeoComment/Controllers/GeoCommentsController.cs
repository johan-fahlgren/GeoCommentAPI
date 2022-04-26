using GeoComment.Data;
using GeoComment.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Controllers
{
    [Route("api/geo-comments")]
    [ApiController]
    public class GeoCommentsController : ControllerBase
    {

        private readonly GeoCommentsDBContext _dbContext;

        public GeoCommentsController(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[ApiVersion("0.1")]
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

        public class NewComment
        {
            public string Message { get; set; }
            public string Author { get; set; }
            public decimal Longitude { get; set; }
            public decimal Latitude { get; set; }
        }


        //[ApiVersion("0.1")]
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            Comment? comment = await _dbContext.Comments.FindAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        //[ApiVersion("0.1")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
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

        }


    }


}
