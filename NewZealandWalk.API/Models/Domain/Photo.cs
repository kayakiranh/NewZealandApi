using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.Domain
{
    [Serializable]
    public class Photo
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Name { get; set; }

        [AllowNull]
        [MaxLength(255)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(5)]
        public string Extension { get; set; }

        [Required]
        public long Size { get; set; }

        [Required]
        [MaxLength(255)]
        public string Path { get; set; }

        [NotMapped]
        public IFormFile FormFile { get; set; }
    }
}