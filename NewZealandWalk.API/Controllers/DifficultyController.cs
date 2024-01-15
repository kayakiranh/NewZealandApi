using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos;
using NewZealandWalk.API.Models.Identity.Domain;
using NewZealandWalk.API.Models.NzWalk.Domain;
using NewZealandWalk.API.Repositories;
using System.Net;

namespace NewZealandWalk.API.Controllers
{
    [Authorize(Roles = "Writer,Reader")]
    //[Authorize]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = null)]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly ILogger<DifficultyController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;

        public DifficultyController(ILogger<DifficultyController> logger, IDifficultyRepository difficultyRepository, IMapper mapper, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _difficultyRepository = difficultyRepository;
            _mapper = mapper;
            _userManager = userManager;
        }

        //(HttpGet)https://localhost:7265/api/v1/difficulty
        //(HttpGet)http://localhost:5070/api/v1/difficulty
        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll([FromQuery] string? queryName, [FromQuery] bool? isOrderName, [FromQuery] int? page = null)
        {

            List<Difficulty> difficultyList = await _difficultyRepository.GetAllAsync(queryName, isOrderName, page);
            if (!difficultyList.Any()) return NoContent();

            List<DifficultyDto> difficultyDtoList = _mapper.Map<List<DifficultyDto>>(difficultyList);
            return Ok(difficultyDtoList);
        }

        //(HttpGet)https://localhost:7265/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpGet)http://localhost:5070/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [MapToApiVersion("1.0")]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        //[Authorize]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Difficulty difficulty = await _difficultyRepository.GetByIdAsync(id);
            if (difficulty.Id == Guid.Empty) return NoContent();

            DifficultyDto difficultyDto = _mapper.Map<DifficultyDto>(difficulty);
            return Ok(difficultyDto);
        }

        //(HttpPost)https://localhost:7265/api/v1/difficulty + body
        //(HttpPost)http://localhost:5070/api/v1/difficulty + body
        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateDifficultyDto model)
        {
            Difficulty difficulty = _mapper.Map<Difficulty>(model);
            Difficulty insertedDifficulty = await _difficultyRepository.CreateAsync(difficulty);
            if (insertedDifficulty.Id == Guid.Empty) return Problem("Difficulty Create Error", "", 417);

            return Ok(difficulty);
        }

        //(HttpPut)https://localhost:7265/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        //(HttpPut)http://localhost:5070/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        [MapToApiVersion("1.0")]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDifficultyDto model)
        {
            Difficulty difficulty = _mapper.Map<Difficulty>(model);
            Difficulty updatedDifficulty = await _difficultyRepository.UpdateAsync(id, difficulty);
            if (updatedDifficulty == null) return Problem("Difficulty Update Error", "", 417);
            if (updatedDifficulty.Id == Guid.Empty) return NoContent();

            return Ok(updatedDifficulty);
        }

        //(HttpDelete)https://localhost:7265/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpDelete)http://localhost:5070/api/v1/difficulty/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [MapToApiVersion("1.0")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("Difficulty Delete : {id}", id); return BadRequest(); }

            Difficulty deletedDifficulty = await _difficultyRepository.DeleteAsync(id);
            if (deletedDifficulty == null) return Problem("Difficulty Delete Error", "", 417);
            if (deletedDifficulty.Id == Guid.Empty) return NoContent();

            return Ok(deletedDifficulty);
        }
    }
}