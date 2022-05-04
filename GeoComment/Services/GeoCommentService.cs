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

        public async Task<Comment> FindComment(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;

            return comment;
        }

        public async Task<List<Comment>> FindAllUserComments(string userName)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.Author == userName)
                .ToListAsync();

            return comments;
        }


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


        public async Task<ResponseCommentV0_2> DeleteComment(int id, string userId)
        {

            var user = await _dbContext.Users
                .Include(u => u.Comments)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user is null) return null;

            var comment = user.Comments.FirstOrDefault(c => c.Id == id);

            if (comment is null) return null;

            var responseComment = new ResponseCommentV0_2()
            {
                Id = comment.Id,
                Latitude = comment.Latitude,
                Longitude = comment.Longitude,
                Body = new Body()
                {
                    Author = comment.Author,
                    Title = comment.Title,
                    Message = comment.Message,
                }
            };

            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChangesAsync();

            return responseComment;
        }







    }
}
