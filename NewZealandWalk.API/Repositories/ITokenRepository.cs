using Microsoft.AspNetCore.Identity;

namespace NewZealandWalk.API.Repositories
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
