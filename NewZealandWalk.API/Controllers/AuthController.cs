using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.Models.DataTransferObject.RegisterDto;

namespace NewZealandWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        public AuthController(UserManager<IdentityUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
        {
            IdentityUser identityUser = new IdentityUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            IdentityResult newIdentityUser = await _userManager.CreateAsync(identityUser, model.Password);
            newIdentityUser = await _userManager.AddToRolesAsync(identityUser, model.Roles);
            if (!newIdentityUser.Succeeded) return BadRequest();

            return Ok();
        }
    }
}
