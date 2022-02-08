#region

#endregion

namespace Engine.Hub
{
  public static class HubExtensions
  {
    public static void Sub<T>(this World.World world, Action<T> handler)
    {
      world.Hub.Subscribe(world.Hub, handler);
    }

    public static void UnSub<T>(this World.World world)
    {
      world.Hub.UnSubscribe<T>(world.Hub);
    }

    public static void UnSub<T>(this World.World world, Action<T> handler)
    {
      world.Hub.UnSubscribe(world.Hub, handler);
    }

    public static void Pub<T>(this World.World world, T output = default)
    {
      world.Hub.Publish(output);
    }

    public static void Push<T>(this World.World world, T output = default) where T : new()
    {
      world.Hub.Push(output);
    }

    public static IEnumerable<T> Pull<T>(this World.World world, Type type = default)
    {
      return world.Hub.Pull<T>(type);
    }
  }
}