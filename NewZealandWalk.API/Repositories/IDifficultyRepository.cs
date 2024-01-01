using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface IDifficultyRepository
    {
        Task<List<Difficulty>> GetAllAsync();

        Task<Difficulty> GetByIdAsync(Guid id);

        Task<Difficulty> CreateAsync(Difficulty model);

        Task<Difficulty> UpdateAsync(Guid id, Difficulty model);

        Task<Difficulty> DeleteAsync(Guid id);
    }
}