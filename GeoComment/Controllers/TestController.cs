using GeoComment.Data;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly GeoCommentsDBContext _dbContext;

        public TestController(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        [ApiVersion("0.1")]
        [Route("reset-db")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ReCreateDatabase()
        {
            var deleted =
                await _dbContext.Database.EnsureDeletedAsync();
            var reCreated =
                await _dbContext.Database.EnsureCreatedAsync();

            if (!deleted) return NotFound();
            if (!reCreated) return BadRequest();

            return Ok();
        }


    }
}
