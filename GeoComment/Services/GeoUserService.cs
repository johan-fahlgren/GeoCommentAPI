using GeoComment.Data;
using GeoComment.Models;

namespace GeoComment.Services
{
    public class GeoUserService
    {

        private readonly GeoCommentsDBContext _dbContext;

        public GeoUserService(GeoCommentsDBContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<GeoUser?> FindGeoUser(string id)
        {
            return await _dbContext.Users.FindAsync(id);
        }

    }
}
