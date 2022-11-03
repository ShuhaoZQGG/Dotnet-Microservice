using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PlatformService.Data;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    // [controller] will get the portion of the class name except for Controller, so here it will get name Platforms
    public class PlatformsController: ControllerBase
    {
        private readonly IPlatformRepo _repo;
        private readonly IMapper _mapper;
        public PlatformsController(IPlatformRepo repository, IMapper mapper)
        {
            _repo = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlatformReadDto>>> GetAllPlatforms()
        {
            var platforms = await _repo.GetAllPlatforms();
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

        [HttpGet("{id}", Name = "GetPlatformById")]
        public async Task<ActionResult<PlatformReadDto>> GetPlatformById(int id)
        {
            var platform = await _repo.GetPlatformById(id);
            if (platform == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<PlatformReadDto>(platform));
        }

        [HttpPost]
        public async Task<ActionResult<PlatformReadDto>> CreatePlatform(PlatformCreateDto platformCreateDto)
        {
            Platform platform = _mapper.Map<Platform>(platformCreateDto);
            await _repo.CreatePlatform(platform);
            await _repo.SaveChanges();
            PlatformReadDto platformReadDto = _mapper.Map<PlatformReadDto>(platform);
            // nameof(GetPlatformById) will return what we defined at line 29 Name = "GetPlatformById"
            return CreatedAtRoute(nameof(GetPlatformById), new { Id = platformReadDto.Id } ,platformReadDto);
        }
    }
}
