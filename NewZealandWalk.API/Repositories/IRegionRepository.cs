using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllAsync();

        Task<Region> GetByIdAsync(Guid id);

        Task<Region> CreateAsync(Region model);

        Task<Region> UpdateAsync(Guid id, Region model);

        Task<Region> DeleteAsync(Guid id);
    }
}