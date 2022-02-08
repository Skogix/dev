#region

using Engine.Events;
using Engine.Hub;

#endregion

namespace Engine.Component
{
  public static class ComponentFactory
  {
    internal static T CreateComponent<T>(Entity.Entity entity) where T : class, new()
    {
      var data = new T();
      var component = new Component(entity.Hash, typeof(T).Name, data);

#if DEBUG
      entity.World.Pub(new Debug($"Factory skapade #{component.Id} åt #{entity.Hash}"));
#endif
      entity.Components.Add(component.Id);
      entity.World.Pub(new ComponentAdded(component, entity));

      return (T) component.Data;
    }
  }
}