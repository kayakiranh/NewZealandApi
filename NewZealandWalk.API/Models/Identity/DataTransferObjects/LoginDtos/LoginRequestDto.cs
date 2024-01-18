using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.Identity.DataTransferObjects.LoginDtos
{
    [Serializable]
    public record LoginRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}