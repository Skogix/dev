#region

using Engine.Component;
using Engine.Entity;
using Engine.EntitySystem;
using RogueLikeGame.Data;
using RogueLikeGame.Data.Components;

#endregion

namespace RogueLikeGame
{
  public class TileSystem : EntitySystem, IRunSystem
  {
    public TileSystem()
    {
      //TileMap = World.GetPrototypeEntity<TileMap>();
      AddFilter<Actor>();
      AddFilter<Transform>();
      AddFilter<Walkable>();
    }

    public TileMap TileMap { get; set; }

    public void Run()
    {
      foreach(var entity in Entities)
      {
        var actor = entity.Get<Actor>();
        var transform = entity.Get<Transform>();
        var targetTile = GetTargetTile(actor.direction, transform);
        if(targetTile.Get<Walkable>().Is)
        {
          foreach(var targetTileEntity in targetTile.Entities)
            if(targetTileEntity.Get<Walkable>().Is == false)
              actor.direction = EDirection.None;
          // den är walkable 
        }
        else
        {
          actor.direction = EDirection.None;
        }
      }
    }

    private Tile GetTargetTile(EDirection actorDirection, Transform transform)
    {
      switch(actorDirection)
      {
        case EDirection.None:
          break;
        case EDirection.Up:
          return TileMap.Tiles[transform.X, transform.Y - 1];
          break;
        case EDirection.Down:
          return TileMap.Tiles[transform.X, transform.Y + 1];
          break;
        case EDirection.Left:
          return TileMap.Tiles[transform.X - 1, transform.Y];
          break;
        case EDirection.Right:
          return TileMap.Tiles[transform.X + 1, transform.Y];
          break;
      }

      return TileMap.Tiles[transform.X, transform.Y];
    }

    public override void Init()
    {
      TileMap = World.GetPrototypeEntity<TileMap>();
      // for(var x = 0; x < TileMap.MapWidth; x++)
      // {
      //   var tile = TileMap.Tiles[x, 0];
      //   tile.Get<Drawable>().Glyph = '#';
      //   tile.Get<Walkable>().Is = false;
      // }
    }
  }
}