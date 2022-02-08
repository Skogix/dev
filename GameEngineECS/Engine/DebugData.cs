#region

using System.Linq;
using Engine.Entity;

#endregion

namespace Engine
{
  public class DebugData
  {
    public World.World World;

    public DebugData(World.World world)
    {
      World = world;
    }

    public WorldData GetData()
    {
      return new WorldData
      {
        EntitiesCount = World.EntityManager.Entities.Count,
        ComponentsCount = World.ComponentManager.ComponentsCount,
        ActiveEntitiesCount = World.EntityManager.Entities.Count(e => e.State == EState.Active),
      };
    }

    public class WorldData
    {
      public int EntitiesCount { get; set; }
      public int ComponentsCount { get; set; }
      public int ActiveEntitiesCount { get; set; }
    }
  }
}