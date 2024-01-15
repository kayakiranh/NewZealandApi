using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.NzWalk.Domain
{
    [Serializable]
    public class Region
    {
        [Required]
        public Guid Id { get; set; }

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