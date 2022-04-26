using GeoComment.Data;
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

        [ApiVersion("0.1")]
        [Route("geo-comments")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult> AddComment()
        {
            return Ok();
        }



    }
}
