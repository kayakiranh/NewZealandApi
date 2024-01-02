using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NewZealandWalk.API.CustomActionFilters;
using NewZealandWalk.API.Models.DataTransferObject.WalkRouteDtos;
using NewZealandWalk.API.Models.Domain;
using NewZealandWalk.API.Repositories;

namespace NewZealandWalk.API.Controllers
{
    //https://localhost:7265/swagger/v1/swagger.json
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
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

        //(HttpGet)https://localhost:7265/api/v1/walkroute
        //(HttpGet)http://localhost:5070/api/v1/walkroute
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetAll([FromQuery] string? queryName, [FromQuery] bool? isOrderName, [FromQuery] bool? isLengthOrder, [FromQuery] int? page = null)
        {
            List<WalkRoute> walkRouteList = await _walkRouteRepository.GetAllAsync(queryName, isOrderName, isLengthOrder, page);
            if (!walkRouteList.Any()) return NoContent();

            List<WalkRouteDto> walkRouteDtoList = _mapper.Map<List<WalkRouteDto>>(walkRouteList);
            return Ok(walkRouteDtoList);
        }

        //(HttpGet)https://localhost:7265/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpGet)http://localhost:5070/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [HttpGet("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(204)]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            WalkRoute walkRoute = await _walkRouteRepository.GetByIdAsync(id);
            if (walkRoute.Id == Guid.Empty) return NoContent();

            WalkRouteDto walkRouteDto = _mapper.Map<WalkRouteDto>(walkRoute);
            return Ok(walkRouteDto);
        }

        //(HttpPost)https://localhost:7265/api/v1/walkroute + body
        //(HttpPost)http://localhost:5070/api/v1/walkroute + body
        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateWalkRouteDto model)
        {
            WalkRoute walkRoute = _mapper.Map<WalkRoute>(model);
            WalkRoute insertedWalkRoute = await _walkRouteRepository.CreateAsync(walkRoute);
            if (insertedWalkRoute.Id == Guid.Empty) return Problem("WalkRoute Create Error", "", 417);

            return Ok(walkRoute);
        }

        //(HttpPut)https://localhost:7265/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        //(HttpPut)http://localhost:5070/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3 + body
        [HttpPut("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkRouteDto model)
        {
            WalkRoute walkRoute = _mapper.Map<WalkRoute>(model);
            WalkRoute updatedWalkRoute = await _walkRouteRepository.UpdateAsync(id, walkRoute);
            if (updatedWalkRoute == null) return Problem("WalkRoute Create Error", "", 417);
            if (updatedWalkRoute.Id == Guid.Empty) return NoContent();

            return Ok(updatedWalkRoute);
        }

        //(HttpDelete)https://localhost:7265/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        //(HttpDelete)http://localhost:5070/api/v1/walkroute/2a03a45e-8d9f-4083-bb9b-9601da9354b3
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(417)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            if (id == Guid.Empty) { _logger.LogError("WalkRoute Delete : {id}", id); return BadRequest(); }

            WalkRoute deletedWalkRoute = await _walkRouteRepository.DeleteAsync(id);
            if (deletedWalkRoute == null) return Problem("WalkRoute Delete Error", "", 417);
            if (deletedWalkRoute.Id == Guid.Empty) return NoContent();

            return Ok(deletedWalkRoute);
        }
    }
}