using GeoComment.Data;
using GeoComment.Models;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("api")]
    [ApiController]
    public class GeoCommentsController : ControllerBase
    {

        private readonly GeoCommentsDBContext _dbContext;

        public GeoCommentsController(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        //[ApiVersion("0.1")]
        [Route("geo-comments")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<Comment>> AddComment(
            Comment comment)
        {
            if (string.IsNullOrEmpty(comment.Author))
                return BadRequest();
            if (string.IsNullOrEmpty(comment.Message))
                return BadRequest();

            await _dbContext.Comments.AddAsync(comment);
            await _dbContext.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.Id }, comment);
        }

        //[ApiVersion("0.1")]
        [Route("geo-comments")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Comment>> GetComment(int id)
        {
            Comment? comment = await _dbContext.Comments.FindAsync(id);

            if (comment is null)
            {
                return NotFound();
            }

            return comment;
        }

    }
}
