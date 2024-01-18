using NewZealandWalk.API.Models.Identity.Domain;

namespace NewZealandWalk.API.Repositories
{
    /// <summary>
    /// Access token and Refresh token generator repository
    /// </summary>
    public interface ITokenRepository
    {
        Task<Tuple<string, string, DateTime>> CreateTokens(AppUser user, List<string> roles);

        Task<Tuple<AppUser, string, string, DateTime>?> CreateNewTokens(string refreshToken);
    }
}