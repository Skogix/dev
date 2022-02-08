#region

using Engine.Component;
using Engine.EntitySystem;
using Engine.Hub;
using RogueLikeGame.Data.Components;

#endregion

namespace RogueLikeGame
{
  public class InputSystem : EntitySystem, IRunSystem
  {
    public InputSystem()
    {
      AddFilter<Actor>();
      AddFilter<Player>();
    }
    public override void Init()
    {
      AddFilter<Actor>();
      AddFilter<Player>();
    }
    public void Run()
    {
      foreach(var entity in Entities)
      {
        var actor = entity.Get<Actor>();
        var transform = entity.Get<Transform>();

        var key = Console.ReadKey().KeyChar;
        // var key = 'e';
        if (key == 'a')
          actor.direction = EDirection.Left;
        else if (key is 'e' or 'd')
          actor.direction = EDirection.Right;
        else if (key is ',' or 'w')
          actor.direction = EDirection.Up;
        else if (key is 'o' or 's')
          actor.direction = EDirection.Down;
        else
          actor.direction = EDirection.None;

        World.Push(new MoveEvent(actor, transform));
      }
    }

  }

  public class MoveEvent : IEvent
  {
    public Actor Actor;
    public Transform Transform;

    public MoveEvent(Actor actor, Transform transform)
    {
      Actor = actor;
      Transform = transform;
    }

    public MoveEvent() { }
  }

  public interface IEvent { }

  public enum EDirection
  {
    None,
    Up,
    Down,
    Left,
    Right,
  }
}