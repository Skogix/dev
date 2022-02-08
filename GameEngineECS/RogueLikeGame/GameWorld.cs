#region

using Engine.World;
using RogueLikeGame.Data;

#endregion

namespace RogueLikeGame
{
  public class GameWorld : World
  {
    public int Skogix;
    public TileMap TileMap;

    public GameWorld(int skogix)
    {
      Console.WriteLine("Skogix!");
      Console.WriteLine(skogix);
    }

    public GameWorld() { }
  }
}