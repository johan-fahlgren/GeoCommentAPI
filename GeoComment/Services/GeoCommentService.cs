using GeoComment.Data;
using GeoComment.DTOs;
using GeoComment.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Services
{
    public class GeoCommentService
    {

        private readonly GeoCommentsDBContext _dbContext;
        private readonly UserManager<GeoUser> _userManager;


        public GeoCommentService(GeoCommentsDBContext dbContext, UserManager<GeoUser> userManager)
        {
            _dbContext = dbContext;
            _userManager = userManager;
        }

        /// <summary>
        /// Takes comment data and creates a new comment and adds it to database.
        /// </summary>
        /// <param name="newComment">Comment data to add</param>
        /// <param name="userId">Logged in user Id</param>
        /// <returns>Returns added comment</returns>
        public async Task<Comment> CreateComment(
            NewCommentV0_2 newComment, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (string.IsNullOrWhiteSpace(newComment.Body.Title) ||
                string.IsNullOrWhiteSpace(newComment.Body.Message))
                return null;

            var comment = new Comment()
            {
                Author = user.UserName,
                Title = newComment.Body.Title,
                Message = newComment.Body.Message,
                Longitude = newComment.Longitude,
                Latitude = newComment.Latitude,
                User = user,
            };

            var addComment =
                await _dbContext.Comments.AddAsync(comment);

            await _dbContext.SaveChangesAsync();

            return addComment.Entity;

        }


        /// <summary>
        /// Finds one specific comment in database.
        /// <param name="id">Comment Id</param>
        /// <returns>Returns comment if found</returns>
        public async Task<Comment> FindComment(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;

            return comment;
        }

        /// <summary>
        /// Finds all comments in database added by specific user.
        /// </summary>
        /// <param name="userName">Username</param>
        /// <returns>Returns list of comment from user</returns>
        public async Task<List<Comment>> FindAllUserComments(string userName)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.Author == userName)
                .ToListAsync();

            return comments;
        }

        /// <summary>
        ///  Finds all Geo-Comments in database in specified area range.
        /// </summary>
        /// <param name="minLon">Longitude minimum value</param>
        /// <param name="maxLon">Longitude maximum value</param>
        /// <param name="minLat">Latitude minimum value</param>
        /// <param name="maxLat">Latitude maximum value</param>
        /// <returns>Returns list of comments within area</returns>
        public async Task<List<Comment>> FindAllGeoComments(decimal? minLon, decimal? maxLon, decimal? minLat, decimal? maxLat)
        {
            var comments = await _dbContext.Comments
                .Where(c =>
                    c.Longitude >= minLon &&
                    c.Longitude <= maxLon &&
                    c.Latitude >= minLat &&
                    c.Latitude <= maxLat)
                .ToListAsync();

            return comments;
        }

        /// <summary>
        /// Deletes comment from database if user-id and comment-id matches.
        /// </summary>
        /// <param name="id">Id of comment to delete</param>
        /// <param name="userId">Id of logged in user</param>
        /// <returns>Returns deleted comment data if everything OK, else returns null or exception if id's don't match</returns>
        /// <exception cref="UnauthorizedException"></exception>
        public async Task<Comment> DeleteComment(int id, string userId)
        {

            var comment = await _dbContext.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (comment is null) return null;

            if (comment.User is null || comment.User.Id != userId) throw new UnauthorizedException();

            var responseComment = comment;

            _dbContext.Comments.Remove(comment);
            await _dbContext.SaveChangesAsync();

            return responseComment;
        }

    }
    /// <summary>
    /// Helper exception to get extra return type for ``DeleteComment`` Task.  
    /// </summary>
    public class UnauthorizedException : Exception //Tack Kim :D!
    {

    }
}
