#region

#endregion

namespace Engine.Entity
{
  public class EntityManager
  {
    internal EntityManager(World.World world)
    {
      World = world;
    }

    private World.World World { get; }
    internal List<Entity> Entities { get; } = new List<Entity>();

    internal T GetEntity<T>() where T : Entity, new()
    {
      return EntityFactory.CreateEntity<T>(Entities.Count, World);
    }

    internal T GetPrototypeEntity<T>() where T : Entity, IPrototype, new()
    {
      return EntityFactory.CreatePrototype<T>(Entities.Count, World);
    }

    public void DestroyEntity(Entity entity)
    {
      entity.State = EState.Cached;
      foreach(var entityComponent in entity.Components) World.ComponentManager.RemoveComponent(entityComponent, entity);
    }
  }
}