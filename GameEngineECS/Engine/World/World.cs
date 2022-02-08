using Engine.Component;
using Engine.Entity;
using Engine.EntitySystem;

namespace Engine.World
{
  public abstract class World
  {
    public World()
    {
      Hub = new Hub.Hub(this);
      DebugData = new DebugData(this);
      EntityManager = new EntityManager(this);
      ComponentManager = new ComponentManager(this);
      EntitySystemManager = new EntitySystemManager(this);
    }

    internal Hub.Hub Hub { get; }
    internal DebugData DebugData { get; }
    internal EntityManager EntityManager { get; }
    internal ComponentManager ComponentManager { get; }
    internal EntitySystemManager EntitySystemManager { get; }

    public void Run()
    {
      EntitySystemManager.RunAllSystems();
    }
  }
}