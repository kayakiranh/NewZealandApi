using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.DataTransferObjects.RegionDtos
{
    [Serializable]
    public record UpdateRegionDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Code has to be a minimum of 3 characters")]
        [MaxLength(10, ErrorMessage = "Code has to be a maximum of 10 characters")]
        public string Code { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name has to be a minimum of 3 characters")]
        [MaxLength(30, ErrorMessage = "Name has to be a maximum of 30 characters")]
        public string Name { get; set; }

        [AllowNull]
        [MaxLength(255, ErrorMessage = "ImageUrl has to be a maximum of 255 characters")]
        public string? ImageUrl { get; set; }
    }
}