using Microsoft.AspNetCore.Identity;

namespace NewZealandWalk.API.Models.Identity.Domain
{
    [Serializable]
    public class AppUser : IdentityUser
    {
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}