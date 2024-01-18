using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Entity Framework Repository for "Region" entity
    /// </summary>
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync(string? queryName = null, bool? isOrderName = null, int? page = null);

        Task<Region> GetByIdAsync(Guid id);

        Task<Region> CreateAsync(Region model);

        Task<Region> UpdateAsync(Guid id, Region model);

        Task<Region> DeleteAsync(Guid id);
    }
}