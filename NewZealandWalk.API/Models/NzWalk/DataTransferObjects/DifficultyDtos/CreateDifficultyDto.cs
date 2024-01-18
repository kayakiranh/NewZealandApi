using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos
{
    [Serializable]
    public record CreateDifficultyDto
    {
        [Required]
        [MinLength(3, ErrorMessage = "Name has to be a minimum of 3 characters")]
        [MaxLength(10, ErrorMessage = "Name has to be a maximum of 10 characters")]
        public string Name { get; set; }
    }
}