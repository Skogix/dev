#region

#endregion

namespace Engine.Entity
{
  public class Entity
  {
    public string Hash => $"{Id}-{Gen}";

    internal List<string> Components { get; set; }
    internal int Gen { get; set; }
    internal int Id { get; set; }
    internal World.World World { get; set; }
    internal EState State { get; set; }
  }
}