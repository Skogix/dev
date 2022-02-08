namespace Engine.Events
{
  public class Debug
  {
    public Debug(string message)
    {
      Message = ":::" + message;
    }

    public string Message { get; set; }
  }
}