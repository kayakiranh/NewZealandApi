using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.Identity.DataTransferObject.RefreshDtos
{
    [Serializable]
    public record RefreshRequestDto
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}