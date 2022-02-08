#region

using static Engine.DebugData;

#endregion

namespace Engine.World
{
  public static class WorldExtensions
  {
    public static WorldData GetData(this World world)
    {
      return world.DebugData.GetData();
    }
  }
}