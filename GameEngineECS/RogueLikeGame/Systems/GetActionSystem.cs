#region

using Engine.Component;
using Engine.EntitySystem;
using RogueLikeGame.Data.Components;

#endregion

namespace RogueLikeGame
{
  public class GetActionSystem : EntitySystem, IRunSystem
  {
    public void Run()
    {
      foreach(var entity in Entities)
      {
        var transform = entity.Get<Transform>();
        var actor = entity.Get<Actor>();

        switch(actor.direction)
        {
          case EDirection.None:
            break;
          case EDirection.Up:
            transform.Y--;
            break;
          case EDirection.Down:
            transform.Y++;
            break;
          case EDirection.Left:
            transform.X--;
            break;
          case EDirection.Right:
            transform.X++;
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }
      }
    }

    public override void Init()
    {
      AddFilter<Transform>();
      AddFilter<Actor>();
      AddFilter<Player>();
    }
  }
}