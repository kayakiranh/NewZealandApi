using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    public class EfRegionRepository : IRegionRepository
    {
        private readonly NzwDbContext _context;
        private readonly ILogger<EfRegionRepository> _logger;

        public EfRegionRepository(NzwDbContext context, ILogger<EfRegionRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Region>> GetAllAsync(string? queryName = null, bool? isOrderName = null, int? page = null)
        {
            IQueryable<Region> regionList = _context.Regions.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryName) && isOrderName != null)
            {
                regionList = regionList.Where(x => x.Name.Contains(queryName));

                regionList = (bool)isOrderName ? regionList.OrderBy(x => x.Name) : regionList.OrderByDescending(x => x.Name);
            }

            if (page > 0) regionList = regionList.Skip((int)page * 10).Take(10);

            if (!regionList.Any()) { _logger.LogInformation("EfRegionRepository GetAllAsync"); }

            return await regionList.ToListAsync();
        }

        public async Task<Region> GetByIdAsync(Guid id)
        {
            Region region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                _logger.LogInformation("EfRegionRepository GetByIdAsync : {id}", id);
                return new Region { Id = Guid.Empty };
            }

            return region;
        }

        public async Task<Region> CreateAsync(Region model)
        {
            await _context.Regions.AddAsync(model);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfRegionRepository CreateAsync : {model}", model);
                return new Region { Id = Guid.Empty };
            }

            return model;
        }

        public async Task<Region> UpdateAsync(Guid id, Region model)
        {
            Region region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                _logger.LogInformation("EfRegionRepository UpdateAsync : {id}", id);
                return new Region { Id = Guid.Empty };
            }

            region.Name = model.Name;
            region.Code = model.Code;
            region.ImageUrl = model.ImageUrl;

            _context.Regions.Update(region);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfRegionRepository UpdateAsync : {id}, {model}", id, model);
                return null;
            }
            return model;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            Region region = await _context.Regions.FindAsync(id);
            if (region == null)
            {
                _logger.LogInformation("EfRegionRepository DeleteAsync : {id}", id);
                return new Region { Id = Guid.Empty };
            }

            _context.Regions.Remove(region);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfRegionRepository DeleteAsync : {id}", id);
                return null;
            }
            return region;
        }
    }
}