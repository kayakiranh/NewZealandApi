using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface IDifficultyRepository
    {
        Task<List<Difficulty>> GetAllAsync(string? queryName = null, bool? isOrderName = null, int? page = null);

        Task<Difficulty> GetByIdAsync(Guid id);

        Task<Difficulty> CreateAsync(Difficulty model);

        Task<Difficulty> UpdateAsync(Guid id, Difficulty model);

        Task<Difficulty> DeleteAsync(Guid id);
    }
}