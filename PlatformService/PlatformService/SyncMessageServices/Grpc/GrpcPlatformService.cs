using AutoMapper;
using Grpc.Core;
using PlatformService.Data;

namespace PlatformService.SyncMessageServices.Grpc
{
  public class GrpcPlatformService: GrpcPlatform.GrpcPlatformBase
  {
	private readonly IPlatformRepo _repo;
	private readonly IMapper _mapper;

	public GrpcPlatformService(IPlatformRepo repo, IMapper mapper)
	{
	  _repo = repo;
	  _mapper = mapper;
	}

	public override async Task<PlatformResponse> GetAllPlatforms(GetAllRequest request, ServerCallContext context)
	{
	  var response = new PlatformResponse();
	  var platforms = await _repo.GetAllPlatforms();
	  foreach(var platform in platforms)
	  {
		response.Platform.Add(_mapper.Map<GrpcPlatformModel>(platform));
	  }

	  return await Task.FromResult(response);
	}
  }
}
