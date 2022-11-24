using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
  public class PlatformsProfile : Profile
  {
    private readonly ILogger<PlatformsProfile> logger;
    public PlatformsProfile()
    {
      // Source -> Target
      try
      {
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<PlatformCreateDto, Platform>();
        CreateMap<PlatformReadDto, PlatformPublishedDto>();
        CreateMap<Platform, GrpcPlatformModel>()
          .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id))
          .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
          .ForMember(dest => dest.Publisher, opt => opt.MapFrom(src => src.Publisher));
      }
      catch (Exception ex)
      {
        logger.LogError(ex.Message);
      }

    }
  }
}
