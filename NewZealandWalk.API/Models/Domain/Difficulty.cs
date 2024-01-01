using System.ComponentModel.DataAnnotations;

namespace NewZealandWalk.API.Models.Domain
{
    [Serializable]
    public class Difficulty
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(10)]
        public string Name { get; set; }
    }
}