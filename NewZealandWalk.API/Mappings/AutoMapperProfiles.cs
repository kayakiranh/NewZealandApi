using AutoMapper;
using NewZealandWalk.API.Models.DataTransferObject.DifficultyDtos;
using NewZealandWalk.API.Models.DataTransferObject.RegionDtos;
using NewZealandWalk.API.Models.DataTransferObject.WalkRouteDtos;
using NewZealandWalk.API.Models.Domain;

namespace NewZealandWalk.API.Mappings
{
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