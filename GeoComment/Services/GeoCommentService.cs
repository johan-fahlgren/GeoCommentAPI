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

            var thisComment = new CommentReturn
            {
                Latitude = comment.Latitude,
                Longitude = comment.Longitude,

                Body = new Body
                {
                    Author = comment.Author,
                    Titel = comment.Message,
                    Message = comment.Message,
                },

            };

            return thisComment;
        }
    }
}
