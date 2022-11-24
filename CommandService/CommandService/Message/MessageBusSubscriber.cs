using CommandService.Configurations;
using CommandService.Event;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace CommandService.Message
{
  public class MessageBusSubscriber : BackgroundService
  {
    private readonly string _host;
    private readonly int _port;
    private readonly IEvent _eventProcessor;
    private readonly ILogger<MessageBusSubscriber> _logger;
    private IConnection _connection;
    private IModel _channel;
    private string _queueName;

    public MessageBusSubscriber(IOptions<RabbitMqConfig> rabbitMqConfig, IEvent eventProcessor, ILogger<MessageBusSubscriber> logger)
    { 
      _host = rabbitMqConfig.Value.Host;
      _port = rabbitMqConfig.Value.Port;
      _eventProcessor = eventProcessor;
      _logger = logger;
      InitializeRabbitMq();
    }

    private void InitializeRabbitMq()
    {
      var factory = new ConnectionFactory() { HostName = _host, Port = _port };

      _connection = factory.CreateConnection();
      _channel = _connection.CreateModel();
      _channel.ExchangeDeclare(exchange: "trigger", type: ExchangeType.Fanout);
      _queueName = _channel.QueueDeclare().QueueName;
      _channel.QueueBind(queue: _queueName, exchange: "trigger", routingKey: "");
      _logger.LogInformation("---> Declare exchange, queue and bind them together, listening on Message Bus...");
      _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
      stoppingToken.ThrowIfCancellationRequested();

      var consumer = new EventingBasicConsumer(_channel);

      consumer.Received += (ModuleHandle, ea) => 
      {
        _logger.LogInformation("--> Event Received!");

        var body = ea.Body;
        var notificationMessage = Encoding.UTF8.GetString(body.ToArray());
        _eventProcessor.ProcessEvent(notificationMessage);
      };

      _channel.BasicConsume(queue: _queueName, autoAck: true, consumer: consumer);
      return Task.CompletedTask;
    }

    private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
    {
      _logger.LogError("---> Connection Shutdown");
    }

    public override void Dispose()
    {
      if (_channel.IsOpen)
      {
        _channel.Close();
        _connection.Close();
      }
      base.Dispose();
    }
  }
}
