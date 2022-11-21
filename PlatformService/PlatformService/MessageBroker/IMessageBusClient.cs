using PlatformService.Dtos;

namespace PlatformService.MessageBroker
{
  public interface IMessageBusClient
  {
    void PublishNewPlatform(PlatformPublishedDto platformPublishedDto, string exchange, string routingKey);
  }
}
