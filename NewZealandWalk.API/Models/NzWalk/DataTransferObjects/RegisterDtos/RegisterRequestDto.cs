using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.DataTransferObjects.RegisterDtos
{
    [Serializable]
    public record RegisterRequestDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [MinLength(8, ErrorMessage = "Username has to be a minimum of 3 characters")]
        [MaxLength(50, ErrorMessage = "Username has to be a maximum of 10 characters")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Password has to be a minimum of 3 characters")] //From program.cs:104
        [MaxLength(30, ErrorMessage = "Password has to be a maximum of 10 characters")]
        public string Password { get; set; }

        [Required]
        public string[] Roles { get; set; }
    }
}