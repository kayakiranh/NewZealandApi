using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Entity Framework Repository for "Photo" entity
    /// </summary>
    public interface IPhotoRepository
    {
        Task<Photo> Upload(Photo model);
    }
}