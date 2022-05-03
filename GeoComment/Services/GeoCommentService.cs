using GeoComment.Data;
using GeoComment.DTOs;

namespace GeoComment.Services
{
    public class GeoCommentService
    {

        private readonly GeoCommentsDBContext _dbContext;


        public GeoCommentService(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<object?> FindComment(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;

            CommentReturn thisComment;

            if (string.IsNullOrWhiteSpace(comment.Title))
            {
                var newTitle = comment.Message.Split(" ")[0];

                thisComment = new CommentReturn
                {
                    Latitude = comment.Latitude,
                    Longitude = comment.Longitude,

                    Body = new Body
                    {
                        Author = comment.Author,
                        Title = newTitle,
                        Message = comment.Message,
                    },

                };

                return thisComment;

            }

            thisComment = new CommentReturn
            {
                Latitude = comment.Latitude,
                Longitude = comment.Longitude,

                Body = new Body
                {
                    Author = comment.Author,
                    Title = comment.Title,
                    Message = comment.Message,
                },

            };

            return thisComment;
        }
    }
}
