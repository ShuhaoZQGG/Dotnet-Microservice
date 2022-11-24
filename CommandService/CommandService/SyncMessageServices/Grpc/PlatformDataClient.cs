using AutoMapper;
using CommandService.Configurations;
using CommandService.Models;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using PlatformService;

namespace CommandService.SyncMessageServices.Grpc
{
  public class PlatformDataClient: IPlatformDataClient
  {
	private readonly IOptions<GrpcPlatformConfig> _config;
	private readonly IMapper _mapper;
	private readonly ILogger<PlatformDataClient> _logger;

	public PlatformDataClient(IOptions<GrpcPlatformConfig> config, IMapper mapper, ILogger<PlatformDataClient> logger)
	{
	  _config = config;
	  _mapper = mapper;
	  _logger = logger;
	  _logger.LogInformation($"GRPC URI: {_config.Value.Url}");
	}

	public async Task<IEnumerable<Platform>> ReturnAllPlatforms()
	{
	  _logger.LogInformation($"---> Calling Grpc Service { _config.Value.Url }");
	  var channel = GrpcChannel.ForAddress(_config.Value.Url);
	  var client = new GrpcPlatform.GrpcPlatformClient(channel);
	  var request = new GetAllRequest();

	  try
	  {
		var reply = await client.GetAllPlatformsAsync(request);
		return _mapper.Map<IEnumerable<Platform>>(reply.Platform);
	  }
	  catch (Exception ex)
	  {
		_logger.LogError($"---> Could not call Grpc Server {ex.Message}");
		return null;
	  }
	}
  }
}
