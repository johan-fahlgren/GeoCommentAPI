using GeoComment.DTOs;
using GeoComment.Models;
using GeoComment.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GeoComment.Controllers
{
    [ApiVersion("0.2")]
    [Route("api/geo-comments")]
    [ApiController]
    public class GeoCommentsControllerV0_2 : ControllerBase
    {
        private readonly GeoCommentService _geoCommentService;
        private readonly GeoUserService _geoUserService;


        public GeoCommentsControllerV0_2(GeoCommentService geoCommentService, GeoUserService geoUserService)
        {
            _geoCommentService = geoCommentService;
            _geoUserService = geoUserService;
        }


        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Comment>> AddComment(
            NewCommentV0_2 newComment)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = await _geoUserService.FindGeoUser(userId);

            if (userName is null) return Unauthorized();

            //newComment.Body.Author = userName.UserName;

            var createdComment = await _geoCommentService.CreateComment(newComment, userId);
            if (createdComment == null) return BadRequest();

            var response =
                ResponseCommentV0_2.CreateReturn(createdComment);


            return CreatedAtAction(nameof(GetComment), new { id = response.Id }, response);
        }


        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ResponseCommentV0_2>> GetComment(int id)
        {
            var comment = await _geoCommentService.FindComment(id);

            if (comment is null)
            {
                return NotFound();
            }

            if (string.IsNullOrWhiteSpace(comment.Title))
            {
                comment.Title = comment.Message.Split(" ")[0];
            }

            var thisComment = ResponseCommentV0_2.CreateReturn(comment);

            return Ok(thisComment);
        }


        [HttpGet]
        [Route("{userName}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ResponseCommentV0_2>>
            GetAllUserComments(string userName)
        {
            var comments = await _geoCommentService.FindAllUserComments(userName);

            if (comments.Count == 0) return NotFound();

            var responseList = new List<ResponseCommentV0_2>();

            foreach (var comment in comments)
            {
                var response = ResponseCommentV0_2.CreateReturn(comment);
                responseList.Add(response);
            }

            return Ok(responseList);
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ResponseCache(Duration = 10)]
        public async Task<ActionResult<ResponseCommentV0_2>> FindComments([FromQuery] decimal? minLon, [FromQuery] decimal? maxLon, [FromQuery] decimal? minLat, [FromQuery] decimal? maxLat)
        {
            if (minLon is null || maxLon is null || minLat is null ||
                maxLat is null) return BadRequest();

            var comments = await
                _geoCommentService.FindAllGeoComments(minLon, maxLon,
                    minLat, maxLat);

            if (comments.Count == 0) return NotFound();

            var responseList = new List<ResponseCommentV0_2>();

            foreach (var comment in comments)
            {
                var response = ResponseCommentV0_2.CreateReturn(comment);
                responseList.Add(response);
            }

            return Ok(responseList);

        }

        [Authorize]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Route("{id:int}")]
        public async Task<ActionResult> DeleteComment(int id)
        {
            var user = HttpContext.User;
            var userId = user.FindFirst(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
            var userName = await _geoUserService.FindGeoUser(userId);

            if (userName is null) return Unauthorized();

            var commentExists = await _geoCommentService.FindComment(id);
            if (commentExists is null) return NotFound();

            var commentDeleted = await
                _geoCommentService.DeleteComment(id, userId);

            if (commentDeleted is null) return Unauthorized();

            return Ok(commentDeleted);
        }




    }
}
