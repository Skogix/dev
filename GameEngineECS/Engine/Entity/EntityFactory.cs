#region

#endregion

namespace Engine.Entity
{
  public static class EntityFactory
  {
    public static T CreateEntity<T>(int id, World.World world) where T : Entity, new()
    {
      var output = new T
      {
        Id = id,
        Gen = 0,
        World = world,
        Components = new List<string>(),
        State = EState.Active,
      };
      world.EntityManager.Entities.Add(output);
      return output;
    }

    public static T CreatePrototype<T>(in int entitiesCount, World.World world) where T : Entity, IPrototype, new()
    {
      var output = CreateEntity<T>(entitiesCount, world);
      output.Init(world);
      return output;
    }
  }
}