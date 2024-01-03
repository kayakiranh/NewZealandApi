using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.DataTransferObject.RegisterDto
{
    [Serializable]
    public record RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string[] Roles { get; set; }
    }
}
