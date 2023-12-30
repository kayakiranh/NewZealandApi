using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.Domain
{
    [Serializable]
    public class Region
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(10)]
        public string Code { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [AllowNull]
        [MaxLength(255)]
        public string ImageUrl { get; set; }
    }
}