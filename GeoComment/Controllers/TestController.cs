using GeoComment.Data;
using Microsoft.AspNetCore.Mvc;

namespace GeoComment.Controllers
{
    [Route("test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly GeoCommentsDBContext _dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<GeoCommentsDBContext> _logger;

        public TestController(GeoCommentsDBContext dbContext, ILogger<GeoCommentsDBContext> logger, IWebHostEnvironment webHostEnvironment)
        {
            _dbContext = dbContext;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [ApiVersion("0.1")]
        [ApiVersion("0.2")]
        [Route("reset-db")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ReCreateDatabase()
        {
            if (_webHostEnvironment.IsDevelopment())
            {
                try
                {
                    var deleted =
                        await _dbContext.Database.EnsureDeletedAsync();
                    if (!deleted) return NotFound();
                }
                catch (OperationCanceledException e)
                {

                    _logger.LogError("Database not deleted", e);
                    return BadRequest();
                }

                try
                {
                    var reCreated = await _dbContext.Database.EnsureCreatedAsync();
                    if (!reCreated) return NotFound();
                }
                catch (OperationCanceledException e)
                {
                    _logger.LogError("Database not created", e);
                    return BadRequest();

                }

                return Ok();
            }

            return BadRequest();

        }


    }
}
