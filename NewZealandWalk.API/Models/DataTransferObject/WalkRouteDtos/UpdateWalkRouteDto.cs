using NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos;
using NewZealandWalk.API.Models.DataTransferObject.RegionDtos;

namespace NewZealandWalk.API.Models.DataTransferObject.WalkRouteDtos
{
    [Serializable]
    public record UpdateWalkRouteDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? ImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }

        public DifficultyDto Difficulty { get; set; }
        public RegionDto RegionDto { get; set; }
    }
}