using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
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

            string filePath = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/Images/{model.Name}{model.Extension}";
            model.Path = filePath;

            await _dbContext.Photos.AddAsync(model);
            int affectedRowCount = await _dbContext.SaveChangesAsync();
            if (affectedRowCount < 1) return new Photo { Id = Guid.Empty };

            return model;
        }
    }
}