namespace NewZealandWalk.API.Models.DataTransferObject.WalkRouteDtos
{
    [Serializable]
    public record CreateWalkRouteDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double LengthInKm { get; set; }
        public string? ImageUrl { get; set; }
        public Guid DifficultyId { get; set; }
        public Guid RegionId { get; set; }
    }
}