using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.Domain
{
    [Serializable]
    public class WalkRoute
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(255)]
        public string Description { get; set; }

        [Required]
        public double LengthInKm { get; set; }

        [AllowNull]
        [MaxLength(255)]
        public string ImageUrl { get; set; }

        //Difficulty-WalkRoute Relation
        public Guid DifficultyId { get; set; }

        public Difficulty Difficulty { get; set; }

        //Region-WalkRoute Relation
        public Guid RegionId { get; set; }

        public Region Region { get; set; }
    }
}