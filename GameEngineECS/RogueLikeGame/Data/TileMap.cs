#region

using Engine.Component;
using Engine.Entity;
using Engine.World;
using RogueLikeGame.Data.Components;

#endregion

namespace RogueLikeGame.Data
{
  public class TileMap : Entity, IPrototype
  {
    public const int MapWidth = 90;
    public const int MapHeight = 10;
    public Tile[,] Tiles = new Tile[MapWidth, MapHeight];

    public void Init(World world)
    {
      Tiles = new Tile[MapWidth, MapHeight];
      for(var x = 0; x < MapWidth; x++)
      {
        for(var y = 0; y < MapHeight; y++)
        {
          Tiles[x, y] = world.GetPrototypeEntity<Tile>();
          Tiles[x, y].Get<Transform>().X = x;
          Tiles[x, y].Get<Transform>().Y = y;
        }
      }
    }

    public Tile GetTile(int x, int y)
    {
      return Tiles[x, y];
    }
  }

  public class Tile : Entity, IPrototype
  {
    public List<Entity> Entities;

    public void Init(World world)
    {
      Entities = new List<Entity>();
      this.Get<Drawable>();
      this.Get<Walkable>().Is = true;
      this.Get<Transform>();
    }
  }

  public class Walkable
  {
    public bool Is;
  }
}