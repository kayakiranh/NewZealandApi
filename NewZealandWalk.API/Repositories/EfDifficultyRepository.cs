using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    public class EfDifficultyRepository : IDifficultyRepository
    {
        private readonly NzwDbContext _context;
        private readonly ILogger<EfDifficultyRepository> _logger;

        public EfDifficultyRepository(NzwDbContext context, ILogger<EfDifficultyRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Difficulty>> GetAllAsync(string? queryName = null, bool? isOrderName = null, int? page = null)
        {
            IQueryable<Difficulty> difficultyList = _context.Difficulties.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(queryName) && isOrderName != null)
            {
                difficultyList = difficultyList.Where(x => x.Name.Contains(queryName));

                difficultyList = (bool)isOrderName ? difficultyList.OrderBy(x => x.Name) : difficultyList.OrderByDescending(x => x.Name);
            }

            if (page > 0) difficultyList = difficultyList.Skip((int)page * 10).Take(10);

            if (!difficultyList.Any()) { _logger.LogInformation("EfDifficultyRepository GetAllAsync"); }

            return await difficultyList.ToListAsync();
        }

        public async Task<Difficulty> GetByIdAsync(Guid id)
        {
            Difficulty difficulty = await _context.Difficulties.FindAsync(id);
            if (difficulty == null)
            {
                _logger.LogInformation("EfDifficultyRepository GetByIdAsync : {id}", id);
                return new Difficulty { Id = Guid.Empty };
            }

            return difficulty;
        }

        public async Task<Difficulty> CreateAsync(Difficulty model)
        {
            await _context.Difficulties.AddAsync(model);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfDifficultyRepository CreateAsync : {model}", model);
                return new Difficulty { Id = Guid.Empty };
            }

            return model;
        }

        public async Task<Difficulty> UpdateAsync(Guid id, Difficulty model)
        {
            Difficulty difficulty = await _context.Difficulties.FindAsync(id);
            if (difficulty == null)
            {
                _logger.LogInformation("EfDifficultyRepository UpdateAsync : {id}", id);
                return new Difficulty { Id = Guid.Empty };
            }

            difficulty.Name = model.Name;

            _context.Difficulties.Update(difficulty);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfDifficultyRepository UpdateAsync : {id}, {model}", id, model);
                return null;
            }
            return model;
        }

        public async Task<Difficulty> DeleteAsync(Guid id)
        {
            Difficulty difficulty = await _context.Difficulties.FindAsync(id);
            if (difficulty == null)
            {
                _logger.LogInformation("EfDifficultyRepository DeleteAsync : {id}", id);
                return new Difficulty { Id = Guid.Empty };
            }

            _context.Difficulties.Remove(difficulty);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("EfDifficultyRepository DeleteAsync : {id}", id);
                return null;
            }
            return difficulty;
        }
    }
}