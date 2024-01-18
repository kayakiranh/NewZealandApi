using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos;
using NewZealandWalk.API.Models.NzWalk.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Authorize(Roles = "Writer,Reader")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class DifficultyController : ControllerBase
    {
        private readonly IDifficultyRepository _difficultyRepository;
        private readonly ILogger<DifficultyController> _logger;
        private readonly IMapper _mapper;

        public DifficultyController(ILogger<DifficultyController> logger, IDifficultyRepository difficultyRepository, IMapper mapper)
        {
            _logger = logger;
            _difficultyRepository = difficultyRepository;
            _mapper = mapper;
        }

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

        [MapToApiVersion("1.0")]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Difficulty difficulty = await _difficultyRepository.GetByIdAsync(id);
            if (difficulty?.Id == Guid.Empty) return NoContent();

            DifficultyDto difficultyDto = _mapper.Map<DifficultyDto>(difficulty);
            return Ok(difficultyDto);
        }

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
            Difficulty? insertedDifficulty = await _difficultyRepository.CreateAsync(difficulty);
            if (insertedDifficulty?.Id == Guid.Empty) return Problem("Difficulty Create Error", "", 417);

            return Ok(difficulty);
        }

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
            Difficulty? updatedDifficulty = await _difficultyRepository.UpdateAsync(id, difficulty);
            if (updatedDifficulty?.Id == Guid.Empty) return Problem("Difficulty Update Error", "", 417);

            return Ok(updatedDifficulty);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("Difficulty Delete : {id}", id); return BadRequest(); }

            Difficulty? deletedDifficulty = await _difficultyRepository.DeleteAsync(id);
            if (deletedDifficulty?.Id == Guid.Empty) return Problem("Difficulty Delete Error", "", 417);

            return Ok(deletedDifficulty);
        }
    }
}