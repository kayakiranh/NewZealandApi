using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Entity Framework Repository for "Photo" entity. It works for local upload. If needed azure blob, must be develop and dependency for azure repository.
    /// </summary>
    [Serializable]
    public class LocalPhotoRepository : IPhotoRepository
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly NzwDbContext _dbContext;

        public LocalPhotoRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NzwDbContext dbContext)
        {
            _webHostEnvironment = webHostEnvironment;
            _httpContextAccessor = httpContextAccessor;
            _dbContext = dbContext;
        }

        public async Task<Photo> Upload(Photo model)
        {
            string localFilePath = Path.Combine(_webHostEnvironment.ContentRootPath, "Images", model.Name + model.Extension);

            using (FileStream fileStream = new FileStream(localFilePath, FileMode.Create))
            {
                await model.FormFile.CopyToAsync(fileStream);
            }

            HttpRequest? httpRequest = _httpContextAccessor?.HttpContext?.Request;
            if (httpRequest == null)
                return new Photo { Id = Guid.Empty };

            model.Path = $"{httpRequest.Scheme}://{httpRequest.Host}{httpRequest.PathBase}/Images/{model.Name}{model.Extension}";

            await _dbContext.Photos.AddAsync(model);
            int affectedRowCount = await _dbContext.SaveChangesAsync();
            if (affectedRowCount < 1)
                return new Photo { Id = Guid.Empty };

            return model;
        }
    }
}