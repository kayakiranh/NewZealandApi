using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.DataTransferObject.LoginDtos
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