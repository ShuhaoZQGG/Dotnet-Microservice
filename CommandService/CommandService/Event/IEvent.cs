namespace CommandService.Event
{
  public interface IEvent
  {
    Task ProcessEvent(string message);
  }
}
