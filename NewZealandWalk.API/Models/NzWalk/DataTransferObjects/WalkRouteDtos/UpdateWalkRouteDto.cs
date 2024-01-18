using NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos;
using NewZealandWalk.API.Models.DataTransferObjects.RegionDtos;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.DataTransferObjects.WalkRouteDtos
{
    [Serializable]
    public record UpdateWalkRouteDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name has to be a minimum of 3 characters")]
        [MaxLength(30, ErrorMessage = "Name has to be a maximum of 30 characters")]
        public string Name { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Description has to be a minimum of 3 characters")]
        [MaxLength(255, ErrorMessage = "Description has to be a maximum of 255 characters")]
        public string Description { get; set; }

        [Required]
        [Range(0.1, 1000, ErrorMessage = "LengthInKm has to be between 0.1 and 1000")]
        public double LengthInKm { get; set; }

        [AllowNull]
        [MaxLength(255, ErrorMessage = "ImageUrl has to be a maximum of 255 characters")]
        public string? ImageUrl { get; set; }

        [Required]
        public Guid DifficultyId { get; set; }

        [Required]
        public Guid RegionId { get; set; }

        public DifficultyDto Difficulty { get; set; }
        public RegionDto RegionDto { get; set; }
    }
}