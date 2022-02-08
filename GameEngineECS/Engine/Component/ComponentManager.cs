#region

using Engine.Events;
using Engine.Hub;

#endregion

namespace Engine.Component
{
  internal class ComponentManager
  {
    private readonly Dictionary<string, Component> _components = new Dictionary<string, Component>();

    private readonly World.World _world;

    internal ComponentManager(World.World world)
    {
      _world = world;
      world.Sub<ComponentAdded>(OnComponentAdded);
    }

    public int ComponentsCount => _components.Count;

    private void OnComponentAdded(ComponentAdded e)
    {
      if(_components.ContainsKey(e.Component.Hash)) return;

      _components.Add(e.Component.Hash, e.Component);
#if DEBUG
      _world.Pub(new Debug($"#{e.Component.Hash} addad till _components"));
#endif
    }

    internal static T CreateComponent<T>(Entity.Entity entity) where T : class, new()
    {
      return ComponentFactory.CreateComponent<T>(entity);
    }

    internal T GetComponentData<T>(Entity.Entity entity) where T : class
    {
      return _components[GetComponentHash<T>(entity)].Data as T;
    }

    internal bool HasComponent<T>(Entity.Entity entity)
    {
      return _components.ContainsKey(GetComponentHash<T>(entity));
    }

    internal void RemoveComponent<T>(Entity.Entity entity) where T : class
    {
      _components.Remove(GetComponentHash<T>(entity));
    }

    private static string GetComponentHash<T>(Entity.Entity entity)
    {
      return $"{entity.Hash}:{typeof(T).Name}";
    }

    internal void RemoveComponent(string entityComponent, Entity.Entity entity)
    {
      _components.Remove($"{entity.Hash}:{entityComponent}");
    }
  }
}