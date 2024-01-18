using NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos;
using NewZealandWalk.API.Models.DataTransferObjects.RegionDtos;

namespace NewZealandWalk.API.Models.DataTransferObjects.WalkRouteDtos
{
    [Serializable]
    public record WalkRouteDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? ImageUrl { get; set; }
        public DifficultyDto Difficulty { get; set; }
        public RegionDto Region { get; set; }
    }
}