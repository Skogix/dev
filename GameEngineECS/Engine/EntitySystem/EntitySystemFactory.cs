#region

using Engine.Events;
using Engine.Hub;

#endregion

namespace Engine.EntitySystem
{
  public class EntitySystemFactory
  {
    private readonly EntitySystemManager _entitySystemManager;

    public EntitySystemFactory(EntitySystemManager entitySystemManager)
    {
      _entitySystemManager = entitySystemManager;
    }

    public EntitySystem CreateEntitySystem<T>(World.World world) where T : EntitySystem, new()
    {
      //var output = new T();
      EntitySystem output = new T();
      //output.Entities = new List<Entity>();
      output.World = world;
      output.Init();
      //output.IncludedFilters = new List<string>();

#if DEBUG
      _entitySystemManager.World.Pub(new Debug($"EntitySystemCreated av typ {output.GetType().Name}."));
#endif
      _entitySystemManager.World.Sub<ComponentAdded>(output.OnComponentAdded);
      _entitySystemManager.World.EntitySystemManager.Systems.Add(output);

      return output;
    }
  }
}