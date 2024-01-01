using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.Models.DataTransferObject.RegionDtos;
using NewZealandWalk.API.Models.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    //https://localhost:7265/swagger/v1/swagger.json
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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

        //(HttpGet)https://localhost:7265/api/v1/region
        //(HttpGet)http://localhost:5070/api/v1/region
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll()
        {
            List<Region> regionList = await _regionRepository.GetAllAsync();
            if (!regionList.Any()) return NoContent();

            List<RegionDto> regionDtoList = _mapper.Map<List<RegionDto>>(regionList);
            return Ok(regionDtoList);
        }

        //(HttpGet)https://localhost:7265/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpGet)http://localhost:5070/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region region = await _regionRepository.GetByIdAsync(id);
            if (region.Id == Guid.Empty) return NoContent();

            RegionDto regionDto = _mapper.Map<RegionDto>(region);
            return Ok(regionDto);
        }

        //(HttpPost)https://localhost:7265/api/v1/region + body
        //(HttpPost)http://localhost:5070/api/v1/region + body
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        public async Task<IActionResult> Create([FromBody] CreateRegionDto model)
        {
            if (model == null) return BadRequest();
            Region region = _mapper.Map<Region>(model);
            Region insertedRegion = await _regionRepository.CreateAsync(region);
            if (insertedRegion.Id == Guid.Empty) return Problem("Region Create Error", "", 417);

            return Ok(region);
        }

        //(HttpPut)https://localhost:7265/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        //(HttpPut)http://localhost:5070/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto model)
        {
            if (model == null) { _logger.LogError("Region Update : {model}", model); return BadRequest(); }
            if (id == Guid.Empty) { _logger.LogError("Region Update : {model}", model); return BadRequest(); }

            Region region = _mapper.Map<Region>(model);
            Region updatedRegion = await _regionRepository.UpdateAsync(id, region);
            if (updatedRegion == null) return Problem("Region Create Error", "", 417);
            if (updatedRegion.Id == Guid.Empty) return NoContent();

            return Ok(updatedRegion);
        }

        //(HttpDelete)https://localhost:7265/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpDelete)http://localhost:5070/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("Region Delete : {id}", id); return BadRequest(); }

            Region deletedRegion = await _regionRepository.DeleteAsync(id);
            if (deletedRegion == null) return Problem("Region Delete Error", "", 417);
            if (deletedRegion.Id == Guid.Empty) return NoContent();

            return Ok(deletedRegion);
        }
    }
}