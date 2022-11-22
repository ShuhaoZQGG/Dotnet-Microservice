namespace CommandService.Event
{
  public interface IEvent
  {
    void ProcessEvent(string message);
  }
}
