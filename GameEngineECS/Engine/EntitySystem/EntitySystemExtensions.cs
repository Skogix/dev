namespace Engine.EntitySystem
{
  public static class EntitySystemExtensions
  {
    public static T AddSystem<T>(this World.World world) where T : EntitySystem, new()
    {
      var output = world.EntitySystemManager.GetEntitySystem<T>(world);
      return output;
    }
  }
}