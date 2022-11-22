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
      }
      catch(Exception ex)
      {
        logger.LogError(ex.Message);
      }

    }
  }
}
