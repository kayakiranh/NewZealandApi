using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewZealandWalk.API.Data;
using NewZealandWalk.API.Models.DataTransferObject.RegionDtos;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class RegionController : ControllerBase
    {
        private readonly NzwDbContext _context;
        private readonly ILogger<RegionController> _logger;

        public RegionController(NzwDbContext context, ILogger<RegionController> logger)
        {
            _context = context;
            _logger = logger;
        }

        //(HttpGet)https://localhost:7265/api/v1/region
        //(HttpGet)http://localhost:5070/api/v1/region
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll()
        {
            List<Region> regionList = await _context.Regions.AsNoTracking().ToListAsync();
            if (!regionList.Any()) { _logger.LogWarning("Region GetById"); return NoContent(); }
            List<RegionDto> regionDtoList = new List<RegionDto>();
            regionList.ForEach(r =>
                regionDtoList.Add(new RegionDto
                {
                    Id = r.Id,
                    Code = r.Code,
                    ImageUrl = r.ImageUrl,
                    Name = r.Name
                }
            ));
            return Ok(regionList);
        }

        //(HttpGet)https://localhost:7265/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpGet)http://localhost:5070/api/v1/region/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            Region region = await _context.Regions.FindAsync(id);
            if (region == null) { _logger.LogWarning("Region Get : {id}", id); return NoContent(); }

            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                ImageUrl = region.ImageUrl,
                Name = region.Name
            };

            return Ok(regionDto);
        }

        //(HttpPost)https://localhost:7265/api/v1 + body
        //(HttpPost)http://localhost:5070/api/v1 + body
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        public async Task<IActionResult> Create([FromBody] CreateRegionDto model)
        {
            if (model == null) return BadRequest();

            Region region = new Region
            {
                Name = model.Name,
                Code = model.Code,
                ImageUrl = model.ImageUrl
            };

            await _context.Regions.AddAsync(region);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("Region Create : {model}", model); return Problem("Region Create Error", "", 417);
            }

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

            Region region = await _context.Regions.FindAsync(id);
            if (region == null) return NoContent();

            region.Name = model.Name;
            region.Code = model.Code;
            region.ImageUrl = model.ImageUrl;

            _context.Regions.Update(region);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("Region Update : {model}", model); return Problem("Region Create Error", "", 417);
            }

            return Ok(region);
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

            Region region = await _context.Regions.FindAsync(id);
            if (region == null) return NoContent();

            _context.Regions.Remove(region);
            int affectedRowCount = await _context.SaveChangesAsync();
            if (affectedRowCount < 0)
            {
                _logger.LogError("Region Delete : {id}", id); return Problem("Region Delete Error", "", 417);
            }

            RegionDto regionDto = new RegionDto
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                ImageUrl = region.ImageUrl
            };

            return Ok(regionDto);
        }
    }
}