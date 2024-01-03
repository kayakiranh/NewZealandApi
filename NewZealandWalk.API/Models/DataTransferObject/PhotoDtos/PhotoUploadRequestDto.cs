using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.DataTransferObject.PhotoDtos
{
    [Serializable]
    public record PhotoUploadRequestDto
    {
        [Required]
        public IFormFile File { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Name has to be a minimum of 3 characters")]
        [MaxLength(255, ErrorMessage = "Name has to be a maximum of 10 characters")]
        public string Name { get; set; }

        [AllowNull]
        [MaxLength(255, ErrorMessage = "Name has to be a maximum of 10 characters")]
        public string? Description { get; set; }

    }
}