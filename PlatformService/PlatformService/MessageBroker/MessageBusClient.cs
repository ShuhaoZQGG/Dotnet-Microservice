using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;

namespace PlatformService.MessageBroker
{
  public class MessageBusClient: IMessageBusClient
  {
    private readonly string _host;
    private readonly int _port;
    private readonly ILogger<MessageBusClient> _logger;
    private readonly IConnection _connection;
    private readonly IModel _channel;
    public MessageBusClient(IOptions<RabbitMq> rabbitmqConfig, ILogger<MessageBusClient> logger)
    {
      _host = rabbitmqConfig.Value.Host;
      _port = rabbitmqConfig.Value.Port;
      _logger = logger;
      var factory = new ConnectionFactory()
      {
        HostName = _host,
        Port = _port
      };
      try
      {
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
        _connection.ConnectionShutdown += RabbitMQ_ConnectionShutDown;
        _logger.LogInformation("---> Connected to Rabbitmq Message Bus");
      }
      catch (Exception ex)
      {
        _logger.LogError($"Error: {ex.Message}");
      }

    }
    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto)
    {
      return;
    }

    private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
    {
      _logger.LogInformation("---> Rabbitmq Connection is shutting down");
    }
  }
}
