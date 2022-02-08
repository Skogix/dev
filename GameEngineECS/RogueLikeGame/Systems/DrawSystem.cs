#region

using Engine.Component;
using Engine.EntitySystem;
using RogueLikeGame.Data.Components;

#endregion

namespace RogueLikeGame
{
  public class DrawSystem : EntitySystem, IRunSystem
  {
    public int DrawnEntities;

    public void Run()
    {
      Console.Clear();
      foreach(var entity in Entities)
      {
        Console.SetCursorPosition(100, 1);
        Console.Write($"drawn entities: {DrawnEntities}");
        var glyph = entity.Get<Drawable>().Glyph;
        var transform = entity.Get<Transform>();

        Console.SetCursorPosition(transform.X, transform.Y);
        Console.Write(glyph);
        DrawnEntities++;
      }
    }

    public override void Init()
    {
      AddFilter<Transform>();
      AddFilter<Drawable>();
    }
  }
}