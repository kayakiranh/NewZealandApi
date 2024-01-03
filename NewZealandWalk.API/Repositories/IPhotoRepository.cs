using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo> Upload(Photo model);
    }
}
