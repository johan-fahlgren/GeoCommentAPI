using GeoComment.Data;
using GeoComment.DTOs;
using GeoComment.Models;
using Microsoft.EntityFrameworkCore;

namespace GeoComment.Services
{
    public class GeoCommentService
    {

        private readonly GeoCommentsDBContext _dbContext;


        public GeoCommentService(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<Comment> CreateComment(
            NewCommentV0_2 newComment)
        {

            if (string.IsNullOrWhiteSpace(newComment.Body.Author) ||
                string.IsNullOrWhiteSpace(newComment.Body.Title) ||
                string.IsNullOrWhiteSpace(newComment.Body.Message))
                return null;

            var comment = new Comment()
            {
                Author = newComment.Body.Author,
                Title = newComment.Body.Title,
                Message = newComment.Body.Message,
                Longitude = newComment.Longitude,
                Latitude = newComment.Latitude,
            };

            var addComment = await _dbContext.Comments.AddAsync(comment);

            await _dbContext.SaveChangesAsync();

            return addComment.Entity;


        }

        public async Task<Comment> FindComment(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;

            return comment;
        }

        public async Task<Comment[]> FindAllUserComments(string userName)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.Author == userName)
                .ToArrayAsync();

            return comments;
        }


        public async Task<Comment[]> FindAllGeoComments(decimal? minLon, decimal? maxLon, decimal? minLat, decimal? maxLat)
        {
            var comments = await _dbContext.Comments
                .Where(c =>
                    c.Longitude >= minLon &&
                    c.Longitude <= maxLon &&
                    c.Latitude >= minLat &&
                    c.Latitude <= maxLat)
                .ToArrayAsync();

            return comments;
        }





    }
}
