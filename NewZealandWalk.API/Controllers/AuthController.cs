using Asp.Versioning;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.Models.DataTransferObject.RegisterDtos;
using NewZealandWalk.API.Models.Identity.DataTransferObject.LoginDtos;
using NewZealandWalk.API.Models.Identity.DataTransferObject.RefreshDtos;
using NewZealandWalk.API.Models.Identity.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    /// <summary>
    /// This API manage auth processes. V1 is standart process, V2 is standart process with client security check
    /// </summary>
    /// <remarks>
    /// Possible values could be:
    ///
    ///     "Freezing", "Bracing", "Chilly", "Cool", "Mild",
    ///     "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    ///
    /// Username must be : xxx@local.com
    /// Password must be : X1x2!x3X4
    /// </remarks>
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
        }

        /// <summary>
        /// This API manage auth processes. V1 is standart process, V2 is standart process with client security check
        /// </summary>
        /// <remarks>
        ///Possible values could be:
        ///
        ///     Username must be : xxx@local.com
        ///     Password must be : X1x2!x3X4
        ///
        /// </remarks>
        /// <param name="model" example="">Describe parameter.</param>
        /// <returns>JSON describing matching product data to the entered barcode</returns>
        [MapToApiVersion("1.0")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterV1([FromBody] RegisterRequestDto model)
        {
            AppUser appUser = new AppUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            IdentityResult newIdentityUser = await _userManager.CreateAsync(appUser, model.Password);
            newIdentityUser = await _userManager.AddToRolesAsync(appUser, model.Roles);
            if (!newIdentityUser.Succeeded) return BadRequest(newIdentityUser.Errors?.First().Description);

            return Ok();
        }

        [MapToApiVersion("1.0")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginV1([FromBody] LoginRequestDto model)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(model.Username);
            if (appUser == null) return BadRequest();

            bool isPasswordTrue = await _userManager.CheckPasswordAsync(appUser, model.Password);
            if (!isPasswordTrue) return BadRequest();

            IList<string> roles = await _userManager.GetRolesAsync(appUser);

            Tuple<string, string, DateTime> tokenData = await _tokenRepository.CreateTokens(appUser, roles.ToList());

            appUser.Token = tokenData.Item1;
            appUser.RefreshToken = tokenData.Item2;
            appUser.RefreshTokenExpiration = tokenData.Item3;

            IdentityResult updateResult = await _userManager.UpdateAsync(appUser);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors?.First().Description);
            }

            await _signInManager.SignInAsync(appUser, true);
            LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item1, RefreshToken = tokenData.Item2, Expiration = tokenData.Item3 };

            return Ok(loginResponseDto);
        }


        [MapToApiVersion("1.0")]
        [HttpPost("Refresh")]
        public async Task<IActionResult> RefreshV1([FromBody] RefreshRequestDto model)
        {
            Tuple<AppUser, string, string, DateTime> tokenData = await _tokenRepository.CreateNewTokens(model.RefreshToken);
            if (tokenData == null)
                return BadRequest();

            tokenData.Item1.Token = tokenData.Item2;
            tokenData.Item1.RefreshToken = tokenData.Item3;
            tokenData.Item1.RefreshTokenExpiration = tokenData.Item4;

            IdentityResult updateResult = await _userManager.UpdateAsync(tokenData.Item1);
            if (!updateResult.Succeeded)
            {
                return BadRequest(updateResult.Errors?.First().Description);
            }

            LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item2, RefreshToken = tokenData.Item3, Expiration = tokenData.Item4 };

            return Ok(loginResponseDto);
        }

        //[MapToApiVersion("2.0")]
        //[HttpPost("Client")]
        //public async Task<IActionResult> ClientV2()
        //{
        //    #region client check

        //    string configClientId = _configuration.GetValue<string>("Client:Id");
        //    string configClientSecret = _configuration.GetValue<string>("Client:Secret");

        //    string auth = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    auth = auth.Replace("Basic ", "");
        //    byte[] data = Convert.FromBase64String(auth);
        //    string decodedAuth = Encoding.UTF8.GetString(data);
        //    string[] authParts = decodedAuth.Split(":");

        //    string clientId = authParts[0];
        //    string clientSecret = authParts[1];

        //    bool isClientIdSame = string.Equals(configClientId, clientId, StringComparison.CurrentCultureIgnoreCase);
        //    bool isClientSecretSame = string.Equals(configClientSecret, clientSecret, StringComparison.CurrentCultureIgnoreCase);

        //    if (!isClientIdSame || !isClientSecretSame) return BadRequest();

        //    #endregion client check

        //    return Ok(auth);
        //}

        //[MapToApiVersion("2.0")]
        //[HttpPost("Register")]
        //public async Task<IActionResult> RegisterV2([FromBody] RegisterRequestDto model)
        //{
        //    IdentityUser identityUser = new IdentityUser
        //    {
        //        UserName = model.Username,
        //        Email = model.Username
        //    };

        //    IdentityResult newIdentityUser = await _userManager.CreateAsync(identityUser, model.Password);
        //    newIdentityUser = await _userManager.AddToRolesAsync(identityUser, model.Roles);
        //    if (!newIdentityUser.Succeeded) return BadRequest();

        //    return Ok();
        //}

        //[MapToApiVersion("2.0")]
        //[HttpPost("Login")]
        //public async Task<IActionResult> LoginV2([FromBody] LoginRequestDto model)
        //{
        //    IdentityUser identityUser = await _userManager.FindByEmailAsync(model.Username);
        //    if (identityUser == null) return BadRequest();

        //    bool isPasswordTrue = await _userManager.CheckPasswordAsync(identityUser, model.Password);
        //    if (!isPasswordTrue) return BadRequest();

        //    IList<string> roles = await _userManager.GetRolesAsync(identityUser);
        //    Tuple<string, string, DateTime> tokenData = await _tokenRepository.CreateTokens(identityUser, roles.ToList());

        //    LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item1, RefreshToken = tokenData.Item2, Expiration = tokenData.Item3 };

        //    return Ok(loginResponseDto);
        //}

        //[MapToApiVersion("3.0")]
        //[HttpPost("Register")]
        //public async Task<IActionResult> RegisterV3([FromBody] RegisterRequestDto model)
        //{
        //    #region client check

        //    string configClientId = _configuration.GetValue<string>("Client:Id");
        //    string configClientSecret = _configuration.GetValue<string>("Client:Secret");

        //    string auth = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    auth = auth.Replace("Basic ", "");
        //    byte[] data = Convert.FromBase64String(auth);
        //    string decodedAuth = Encoding.UTF8.GetString(data);
        //    string[] authParts = decodedAuth.Split(":");

        //    string clientId = authParts[0];
        //    string clientSecret = authParts[1];

        //    bool isClientIdSame = string.Equals(configClientId, clientId, StringComparison.CurrentCultureIgnoreCase);
        //    bool isClientSecretSame = string.Equals(configClientSecret, clientSecret, StringComparison.CurrentCultureIgnoreCase);

        //    if (!isClientIdSame || !isClientSecretSame) return BadRequest();

        //    #endregion client check

        //    IdentityUser identityUser = new IdentityUser
        //    {
        //        UserName = model.Username,
        //        Email = model.Username
        //    };

        //    IdentityResult newIdentityUser = await _userManager.CreateAsync(identityUser, model.Password);
        //    newIdentityUser = await _userManager.AddToRolesAsync(identityUser, model.Roles);
        //    if (!newIdentityUser.Succeeded) return BadRequest();

        //    return Ok();
        //}

        //[MapToApiVersion("3.0")]
        //[HttpPost("Login")]
        //public async Task<IActionResult> LoginV3([FromBody] LoginRequestDto model)
        //{
        //    #region client check

        //    string configClientId = _configuration.GetValue<string>("Client:Id");
        //    string configClientSecret = _configuration.GetValue<string>("Client:Secret");

        //    string auth = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];
        //    auth = auth.Replace("Basic ", "");
        //    byte[] data = Convert.FromBase64String(auth);
        //    string decodedAuth = Encoding.UTF8.GetString(data);
        //    string[] authParts = decodedAuth.Split(":");

        //    string clientId = authParts[0];
        //    string clientSecret = authParts[1];

        //    bool isClientIdSame = string.Equals(configClientId, clientId, StringComparison.CurrentCultureIgnoreCase);
        //    bool isClientSecretSame = string.Equals(configClientSecret, clientSecret, StringComparison.CurrentCultureIgnoreCase);

        //    if (!isClientIdSame || !isClientSecretSame) return BadRequest();

        //    #endregion client check

        //    IdentityUser identityUser = await _userManager.FindByEmailAsync(model.Username);
        //    if (identityUser == null) return BadRequest();

        //    bool isPasswordTrue = await _userManager.CheckPasswordAsync(identityUser, model.Password);
        //    if (!isPasswordTrue) return BadRequest();

        //    IList<string> roles = await _userManager.GetRolesAsync(identityUser);
        //    Tuple<string, string, DateTime> tokenData = await _tokenRepository.CreateTokens(identityUser, roles.ToList());

        //    LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item1, RefreshToken = tokenData.Item2, Expiration = tokenData.Item3 };

        //    return Ok(loginResponseDto);
        //}
    }
}
