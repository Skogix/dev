#region

#endregion

namespace Engine.EntitySystem
{
  public class EntitySystemManager
  {
    public EntitySystemManager(World.World world)
    {
      World = world;
      Factory = new EntitySystemFactory(this);
      Systems = new List<EntitySystem>();
    }

    internal World.World World { get; }
    private EntitySystemFactory Factory { get; }

       // foreach (var item in skogix.Inventory.Items)
      // {
      //   Console.WriteLine($"Item: {item}");
      // }
   internal List<EntitySystem> Systems { get; }

    private List<IRunSystem> RunSystems { get; } = new List<IRunSystem>();
    /*
    internal List<IPushSystem> PushSystems { get; }
    internal List<IRespondSystem> RespondSystems { get; }
    */

    public T GetEntitySystem<T>(World.World world) where T : EntitySystem, new()
    {
      var output = Factory.CreateEntitySystem<T>(world);
      if(output is IRunSystem runSystem) RunSystems.Add(runSystem);
      return (T) output;
    }

    public void RunAllSystems()
    {
      foreach(var runSystem in RunSystems) runSystem.Run();
    }
  }
}