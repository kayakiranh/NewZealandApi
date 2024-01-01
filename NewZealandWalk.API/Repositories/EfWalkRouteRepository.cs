using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public class EfWalkRouteRepository : IWalkRouteRepository
    {
        private readonly NzwDbContext _context;
        private readonly ILogger<EfWalkRouteRepository> _logger;

        public EfWalkRouteRepository(NzwDbContext context, ILogger<EfWalkRouteRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<WalkRoute>> GetAllAsync()
        {
            List<WalkRoute> walkRouteList = await _context.WalkRoutes.Include(x => x.Difficulty).Include(x => x.Region).AsNoTracking().ToListAsync();
            if (!walkRouteList.Any()) { _logger.LogInformation("EfWalkRouteRepository GetAllAsync"); }

            return walkRouteList;
        }

        public async Task<WalkRoute> GetByIdAsync(Guid id)
        {
            WalkRoute walkRoute = await _context.WalkRoutes.Include(x => x.Difficulty).Include(x => x.Region).FirstOrDefaultAsync(x => x.Id == id);
            if (walkRoute == null)
            {
                _logger.LogInformation("EfWalkRouteRepository GetByIdAsync : {id}", id);
                return new WalkRoute { Id = Guid.Empty };
            }

            return walkRoute;
        }

        public async Task<WalkRoute> CreateAsync(WalkRoute model)
        {
            Region region = await _context.Regions.FindAsync(model.RegionId);
            if (region == null)
            {
                _logger.LogError("EfWalkRouteRepository CreateAsync : {region}", region);
                return new WalkRoute { Id = Guid.Empty };
            }

            Difficulty difficulty = await _context.Difficulties.FindAsync(model.DifficultyId);
            if (region == null)
            {
                _logger.LogError("EfWalkRouteRepository CreateAsync : {difficulty}", difficulty);
                return new WalkRoute { Id = Guid.Empty };
            }

            await _context.WalkRoutes.AddAsync(model);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfWalkRouteRepository CreateAsync : {model}", model);
                return new WalkRoute { Id = Guid.Empty };
            }

            return model;
        }

        public async Task<WalkRoute> UpdateAsync(Guid id, WalkRoute model)
        {
            WalkRoute walkRoute = await _context.WalkRoutes.FindAsync(id);
            if (walkRoute == null)
            {
                _logger.LogInformation("EfWalkRouteRepository UpdateAsync : {id}", id);
                return new WalkRoute { Id = Guid.Empty };
            }

            Region region = await _context.Regions.FindAsync(model.RegionId);
            if (region == null)
            {
                _logger.LogError("EfWalkRouteRepository CreateAsync : {region}", region);
                return new WalkRoute { Id = Guid.Empty };
            }

            Difficulty difficulty = await _context.Difficulties.FindAsync(model.DifficultyId);
            if (region == null)
            {
                _logger.LogError("EfWalkRouteRepository CreateAsync : {difficulty}", difficulty);
                return new WalkRoute { Id = Guid.Empty };
            }

            walkRoute.Description = model.Description;
            walkRoute.RegionId = model.RegionId;
            walkRoute.DifficultyId = model.DifficultyId;
            walkRoute.ImageUrl = model.ImageUrl;
            walkRoute.LengthInKm = model.LengthInKm;
            walkRoute.Name = model.Name;

            _context.WalkRoutes.Update(walkRoute);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfWalkRouteRepository UpdateAsync : {id}, {model}", id, model);
                return null;
            }
            return model;
        }

        public async Task<WalkRoute> DeleteAsync(Guid id)
        {
            WalkRoute walkRoute = await _context.WalkRoutes.FindAsync(id);
            if (walkRoute == null)
            {
                _logger.LogInformation("EfWalkRouteRepository DeleteAsync : {id}", id);
                return new WalkRoute { Id = Guid.Empty };
            }

            _context.WalkRoutes.Remove(walkRoute);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfWalkRouteRepository DeleteAsync : {id}", id);
                return null;
            }
            return walkRoute;
        }
    }
}