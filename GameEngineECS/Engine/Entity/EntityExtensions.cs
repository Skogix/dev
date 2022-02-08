namespace Engine.Entity
{
  public static class EntityExtensions
  {
    public static Entity GetEntity(this World.World world)
    {
      return world.EntityManager.GetEntity<Entity>();
    }

    public static T GetPrototypeEntity<T>(this World.World world) where T : Entity, IPrototype, new()
    {
      return world.EntityManager.GetPrototypeEntity<T>();
    }

    public static void Destroy(this Entity entity)
    {
      entity.World.EntityManager.DestroyEntity(entity);
    }
  }
}