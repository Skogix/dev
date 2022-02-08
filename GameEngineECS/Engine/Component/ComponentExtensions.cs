namespace Engine.Component
{
  public static class ComponentExtensions
  {
    public static T Get<T>(this Entity.Entity entity) where T : class, new()
    {
      if(entity.Has<T>())
      {
        var output = entity.World.ComponentManager.GetComponentData<T>(entity);
        return output;
      }
      else
      {
        var output = ComponentManager.CreateComponent<T>(entity);
        return output;
      }
    }

    public static bool Has<T>(this Entity.Entity entity) where T : class
    {
      return entity.World.ComponentManager.HasComponent<T>(entity);
    }

    public static void Remove<T>(this Entity.Entity entity) where T : class
    {
      if(entity.Has<T>()) entity.World.ComponentManager.RemoveComponent<T>(entity);
    }
  }
}