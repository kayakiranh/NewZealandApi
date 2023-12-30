namespace NewZealandWalk.API.Models.DataTransferObject.RegionDtos
{
    [Serializable]
    public record CreateRegionDto
    {
        public string Code { get; set; }

        public string Name { get; set; }
        public string? ImageUrl { get; set; }
    }
}