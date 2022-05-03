using GeoComment.Data;
using GeoComment.Models;

namespace GeoComment.Services
{
    public class GeoCommentService
    {

        private readonly GeoCommentsDBContext _dbContext;


        public GeoCommentService(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Comment> FindComment(int id)
        {
            var comment = await _dbContext.Comments.FindAsync(id);
            if (comment == null) return null;

            return comment;
        }
    }
}
