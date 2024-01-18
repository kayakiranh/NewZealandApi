using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.DataTransferObjects.RegionDtos;
using NewZealandWalk.API.Models.NzWalk.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    [Authorize(Roles = "Writer,Reader")]
    [ApiController]
    [Produces("application/json")]
    [ApiVersion("1.0")]
    [Route("api/v{VersionId:apiVersion}/[controller]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionRepository _regionRepository;
        private readonly ILogger<RegionController> _logger;
        private readonly IMapper _mapper;

        public RegionController(ILogger<RegionController> logger, IRegionRepository regionRepository, IMapper mapper)
        {
            _logger = logger;
            _regionRepository = regionRepository;
            _mapper = mapper;
        }

        [MapToApiVersion("1.0")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetAll([FromQuery] string? queryName, [FromQuery] bool? isOrderName, [FromQuery] int? page = null)
        {
            List<Region> regionList = await _regionRepository.GetAllAsync(queryName, isOrderName, page);
            if (!regionList.Any()) return NoContent();

            List<RegionDto> regionDtoList = _mapper.Map<List<RegionDto>>(regionList);
            return Ok(regionDtoList);
        }

        [MapToApiVersion("1.0")]
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        [Authorize(Roles = "Writer,Reader")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region? region = await _regionRepository.GetByIdAsync(id);
            if (region?.Id == Guid.Empty) return NoContent();

            RegionDto regionDto = _mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        [MapToApiVersion("1.0")]
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Create([FromBody] CreateRegionDto model)
        {
            Region region = _mapper.Map<Region>(model);
            Region? insertedRegion = await _regionRepository.CreateAsync(region);
            if (insertedRegion?.Id == Guid.Empty) return Problem("Region Create Error", "", 417);

            return Ok(region);
        }

        [MapToApiVersion("1.0")]
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto model)
        {
            Region region = _mapper.Map<Region>(model);
            Region? updatedRegion = await _regionRepository.UpdateAsync(id, region);
            if (updatedRegion?.Id == Guid.Empty) return Problem("Region Create Error", "", 417);

            return Ok(updatedRegion);
        }

        [MapToApiVersion("1.0")]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("Region Delete : {id}", id); return BadRequest(); }

            Region? deletedRegion = await _regionRepository.DeleteAsync(id);
            if (deletedRegion?.Id == Guid.Empty) return Problem("Region Delete Error", "", 417);

            return Ok(deletedRegion);
        }
    }
}