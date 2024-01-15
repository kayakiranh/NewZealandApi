using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface IPhotoRepository
    {
        Task<Photo> Upload(Photo model);
    }
}