namespace Engine.Events
{
  internal class ComponentAdded
  {
    public readonly Component.Component Component;
    internal readonly Entity.Entity Entity;

    internal ComponentAdded(Component.Component component, Entity.Entity entity)
    {
      Component = component;
      Entity = entity;
    }
  }
}