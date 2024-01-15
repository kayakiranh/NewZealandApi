using NewZealandWalk.API.Models.Identity.Domain;

namespace NewZealandWalk.API.Repositories
{
    public interface ITokenRepository
    {
        Task<Tuple<string, string, DateTime>> CreateTokens(AppUser user, List<string> roles);

        Task<Tuple<AppUser, string, string, DateTime>> CreateNewTokens(string refreshToken);
    }
}