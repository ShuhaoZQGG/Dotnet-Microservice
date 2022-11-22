using Microsoft.Extensions.Options;
using PlatformService.Configuration;
using PlatformService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

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
      logger.LogInformation($"rabbitmq hotst {_host}");
      logger.LogInformation($"rabbitmq port {_port}");

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
    public void PublishNewPlatform(PlatformPublishedDto platformPublishedDto, string exchange, string routingKey)
    {
      try
      {
        var message = JsonSerializer.Serialize(platformPublishedDto);
        _logger.LogInformation(message);
        if (_connection.IsOpen)
        {
          _logger.LogInformation("---> RabbitMq connection is open, sending message...");
          SendMessage(exchange, routingKey, message);
        }
        else
        {
          _logger.LogInformation("---> RabbitMq connection is closed, send nothing");
        }
      } 
      catch (Exception ex)
      {
        _logger.LogError($"Error: {ex.Message}");
      }

    }

    private void SendMessage(string exchange, string routingKey, string message)
    {
      var body = Encoding.UTF8.GetBytes(message);
      _channel.BasicPublish(exchange, routingKey, basicProperties: null, body);
      _logger.LogInformation($"---> We have send the message {message}");
    }
    
    private void Dispose()
    {
      _logger.LogInformation("Message bus disposed");
      if (_channel.IsOpen)
      {
        _channel.Close();
        _channel.Dispose();
      }
    }
    private void RabbitMQ_ConnectionShutDown(object sender, ShutdownEventArgs e)
    {
      _logger.LogInformation("---> Rabbitmq Connection is shutting down");
    }
  }
}
