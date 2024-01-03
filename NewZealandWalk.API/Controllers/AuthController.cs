using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.Models.DataTransferObject.LoginDtos;
using NewZealandWalk.API.Models.DataTransferObject.RegisterDtos;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
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

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            IdentityUser identityUser = await _userManager.FindByEmailAsync(model.Username);
            if (identityUser == null) return BadRequest();

            bool isPasswordTrue = await _userManager.CheckPasswordAsync(identityUser, model.Password);
            if (!isPasswordTrue) return BadRequest();

            IList<string> roles = await _userManager.GetRolesAsync(identityUser);
            string token = _tokenRepository.CreateJwtToken(identityUser, roles.ToList());

            LoginResponseDto loginResponseDto = new LoginResponseDto { JwtToken = token };

            return Ok(loginResponseDto);
        }
    }
}
