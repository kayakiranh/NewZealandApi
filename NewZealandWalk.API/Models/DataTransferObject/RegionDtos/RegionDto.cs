using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace NewZealandWalk.API.Models.DataTransferObject.RegionDtos
{
    [Serializable]
    public record RegionDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; }

        public string Name { get; set; }
        public string ImageUrl { get; set; }
    }
}