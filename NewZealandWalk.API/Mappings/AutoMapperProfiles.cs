using AutoMapper;
using NewZealandWalk.API.Models.DataTransferObjects.DifficultyDtos;
using NewZealandWalk.API.Models.DataTransferObjects.RegionDtos;
using NewZealandWalk.API.Models.DataTransferObjects.WalkRouteDtos;
using NewZealandWalk.API.Models.NzWalk.Domain;

namespace NewZealandWalk.API.Mappings
{
    /// <summary>
    /// AutoMapper mapping for DTO <> Entity
    /// </summary>
    [Serializable]
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<Region, CreateRegionDto>().ReverseMap();
            CreateMap<Region, UpdateRegionDto>().ReverseMap();
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();
            CreateMap<Difficulty, CreateDifficultyDto>().ReverseMap();
            CreateMap<Difficulty, UpdateDifficultyDto>().ReverseMap();
            CreateMap<WalkRoute, WalkRouteDto>().ReverseMap();
            CreateMap<WalkRoute, CreateWalkRouteDto>().ReverseMap();
            CreateMap<WalkRoute, UpdateWalkRouteDto>().ReverseMap();
        }
    }
}