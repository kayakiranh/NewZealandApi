using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.DataTransferObjects.WalkRouteDtos;
using NewZealandWalk.API.Models.NzWalk.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Authorize(Roles = "Writer,Reader")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class WalkRouteController : ControllerBase
    {
        private readonly IWalkRouteRepository _walkRouteRepository;
        private readonly ILogger<WalkRouteController> _logger;
        private readonly IMapper _mapper;

        public WalkRouteController(ILogger<WalkRouteController> logger, IWalkRouteRepository walkRouteRepository, IMapper mapper)
        {
            _logger = logger;
            _walkRouteRepository = walkRouteRepository;
            _mapper = mapper;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetAll([FromQuery] string? queryName, [FromQuery] bool? isOrderName, [FromQuery] bool? isLengthOrder, [FromQuery] int? page = null)
        {
            List<WalkRoute> walkRouteList = await _walkRouteRepository.GetAllAsync(queryName, isOrderName, isLengthOrder, page);
            if (!walkRouteList.Any()) return NoContent();

            List<WalkRouteDto> walkRouteDtoList = _mapper.Map<List<WalkRouteDto>>(walkRouteList);
            return Ok(walkRouteDtoList);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            WalkRoute walkRoute = await _walkRouteRepository.GetByIdAsync(id);
            if (walkRoute?.Id == Guid.Empty) return NoContent();

            WalkRouteDto walkRouteDto = _mapper.Map<WalkRouteDto>(walkRoute);
            return Ok(walkRouteDto);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] CreateWalkRouteDto model)
        {
            WalkRoute walkRoute = _mapper.Map<WalkRoute>(model);
            WalkRoute insertedWalkRoute = await _walkRouteRepository.CreateAsync(walkRoute);
            if (insertedWalkRoute.Id == Guid.Empty) return Problem("WalkRoute Create Error", "", 417);

            return Ok(walkRoute);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRouteDto model)
        {
            WalkRoute walkRoute = _mapper.Map<WalkRoute>(model);
            WalkRoute updatedWalkRoute = await _walkRouteRepository.UpdateAsync(id, walkRoute);
            if (updatedWalkRoute?.Id == Guid.Empty) return Problem("WalkRoute Create Error", "", 417);

            return Ok(updatedWalkRoute);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("WalkRoute Delete : {id}", id); return BadRequest(); }

            WalkRoute deletedWalkRoute = await _walkRouteRepository.DeleteAsync(id);
            if (deletedWalkRoute?.Id == Guid.Empty) return Problem("WalkRoute Delete Error", "", 417);

            return Ok(deletedWalkRoute);
        }
    }
}