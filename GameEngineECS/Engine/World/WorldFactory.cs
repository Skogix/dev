namespace Engine.World
{
  public class WorldFactory
  {
    public static T CreateWorld<T>() where T : new()
    {
      var output = new T();
      return output;
    }
  }
}