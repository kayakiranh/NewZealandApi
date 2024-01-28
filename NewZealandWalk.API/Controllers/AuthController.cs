using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NewZealandWalk.API.Attributes;
using NewZealandWalk.API.Models.DataTransferObjects.RegisterDtos;
using NewZealandWalk.API.Models.Identity.DataTransferObjects.LoginDtos;
using NewZealandWalk.API.Models.Identity.DataTransferObjects.RefreshDtos;
using NewZealandWalk.API.Models.Identity.Domain;
using NewZealandWalk.API.Repositories;
using System.Text;

namespace NewZealandWalk.API.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [ApiVersion("3.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenRepository _tokenRepository;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthController(UserManager<AppUser> userManager, ITokenRepository tokenRepository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, SignInManager<AppUser> signInManager, ILogger logger)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _logger = logger;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <remarks>
        /// Example Request :
        /// <code>
        /// {
        ///     "Username": "swagger1@local.com",
        ///     "Password": "X1x2!x3X4",
        ///     "Roles": [
        ///       "Writer","Reader"
        ///     ]
        /// }
        /// Example Response :
        /// 200/400+ string
        ///</code>
        /// </remarks>
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

        /// <summary>
        /// Login
        /// </summary>
        /// <remarks>
        /// Example Request :
        /// <code>
        /// {
        ///     "Username": "swagger1@local.com",
        ///     "Password": "X1x2!x3X4"
        /// }
        ///</code>
        /// Example Response :
        /// <code>
        /// {
        ///     "Token": "eyJhbGciOiJodHRwOi8vdYcXlPc2pfaHYOFZLMmU0b3pbXOC",
        ///     "RefreshToken": "eyJhbGciOiJodHRwOi8v9yZy93cy8yMDA1LzA12piG",
        ///     "Expiration": "2024-01-18T17:25:48.2869957+03:00"
        /// }
        /// </code>
        /// </remarks>
        [MapToApiVersion("1.0")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginV1([FromBody] LoginRequestDto model)
        {
            AppUser? appUser = await _userManager.FindByEmailAsync(model.Username);
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

        /// <summary>
        /// Get new tokens with valid refresh token
        /// </summary>
        /// <remarks>
        /// Example Request :
        /// <code>
        /// {
        ///    "RefreshToken": "eyJhbGciOiJodHRwOi8v9yZy93cy8yMDA1LzA12piG",
        /// }
        ///</code>
        /// Example Response :
        /// <code>
        /// {
        ///     "Token": "eyJhbGciOiJodHRwOi8vdYcXlPc2pfaHYOFZLMmU0b3pbXOC",
        ///     "RefreshToken": "eyJhbGciOiJodHRwOi8v9yZy93cy8yMDA1LzA12piG",
        ///     "Expiration": "2024-01-18T17:25:48.2869957+03:00"
        /// }
        /// </code>
        /// </remarks>
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

        /// <summary>
        /// Get client token with basic auth
        /// </summary>
        /// <remarks>
        /// Example Request :
        /// <code>
        /// {
        ///     "Username": "aW5waZSb2GlzyZB2F5jbw==",
        ///     "Password": "IVBoaS5yZbDS5T0dmb0b23s2Z0d2MX0h",
        /// }
        /// Example Response :
        /// 200/400+ string
        ///</code>
        /// </remarks>
        [MapToApiVersion("2.0")]
        [HttpPost("Client")]
        public async Task<IActionResult> ClientV2()
        {
            #region client check

            string? configClientId = _configuration.GetValue<string>("Client:Id");
            string? configClientSecret = _configuration.GetValue<string>("Client:Secret");
            if (string.IsNullOrEmpty(configClientId) || string.IsNullOrEmpty(configClientSecret)) return BadRequest();

            string? auth = _httpContextAccessor?.HttpContext?.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(auth)) return Unauthorized();

            auth = auth.Replace("Basic ", "");
            byte[] data = Convert.FromBase64String(auth);
            string decodedAuth = Encoding.UTF8.GetString(data);
            string[] authParts = decodedAuth.Split(":");

            string clientId = authParts[0];
            string clientSecret = authParts[1];

            bool isClientIdSame = string.Equals(configClientId, clientId, StringComparison.CurrentCultureIgnoreCase);
            bool isClientSecretSame = string.Equals(configClientSecret, clientSecret, StringComparison.CurrentCultureIgnoreCase);

            if (!isClientIdSame || !isClientSecretSame) return BadRequest();

            #endregion client check

            string clientToken = await _tokenRepository.CreateClientToken();
            return Ok(clientToken);
        }

        [TypeFilter(typeof(ClientTokenAttribute))]
        [MapToApiVersion("2.0")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterV2([FromBody] RegisterRequestDto model)
        {
            AppUser appUser = new AppUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, model.Password);
            identityResult = await _userManager.AddToRolesAsync(appUser, model.Roles);
            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            return Ok();
        }

        [TypeFilter(typeof(ClientTokenAttribute))]
        [MapToApiVersion("2.0")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginV2([FromBody] LoginRequestDto model)
        {
            AppUser? appUser = await _userManager.FindByEmailAsync(model.Username);
            if (appUser == null) return BadRequest();

            bool isPasswordTrue = await _userManager.CheckPasswordAsync(appUser, model.Password);
            if (!isPasswordTrue) return BadRequest();

            IList<string> roles = await _userManager.GetRolesAsync(appUser);
            Tuple<string, string, DateTime> tokenData = await _tokenRepository.CreateTokens(appUser, roles.ToList());

            LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item1, RefreshToken = tokenData.Item2, Expiration = tokenData.Item3 };

            return Ok(loginResponseDto);
        }

        [TypeFilter(typeof(ClientCheckAttribute))]
        [MapToApiVersion("3.0")]
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterV3([FromBody] RegisterRequestDto model)
        {
            AppUser appUser = new AppUser
            {
                UserName = model.Username,
                Email = model.Username
            };

            IdentityResult identityResult = await _userManager.CreateAsync(appUser, model.Password);
            identityResult = await _userManager.AddToRolesAsync(appUser, model.Roles);
            if (!identityResult.Succeeded) return BadRequest(identityResult.Errors);

            return Ok();
        }

        [TypeFilter(typeof(ClientCheckAttribute))]
        [MapToApiVersion("3.0")]
        [HttpPost("Login")]
        public async Task<IActionResult> LoginV3([FromBody] LoginRequestDto model)
        {
            AppUser? appUser = await _userManager.FindByEmailAsync(model.Username);
            if (appUser == null) return BadRequest();

            bool isPasswordTrue = await _userManager.CheckPasswordAsync(appUser, model.Password);
            if (!isPasswordTrue) return BadRequest();

            IList<string> roles = await _userManager.GetRolesAsync(appUser);
            Tuple<string, string, DateTime> tokenData = await _tokenRepository.CreateTokens(appUser, roles.ToList());

            LoginResponseDto loginResponseDto = new LoginResponseDto { Token = tokenData.Item1, RefreshToken = tokenData.Item2, Expiration = tokenData.Item3 };

            return Ok(loginResponseDto);
        }
    }
}