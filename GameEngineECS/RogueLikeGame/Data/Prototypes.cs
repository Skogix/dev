#region

using Engine.Component;
using Engine.Entity;
using Engine.World;
using RogueLikeGame.Data.Components;
using RogueLikeGame.Interfaces;

#endregion

namespace RogueLikeGame.Data
{
  public class Skogix : Entity, IPrototype
  {
    public Inventory Inventory;

    public void Init(World world)
    {
      this.Get<Player>();
      this.Get<Transform>().X = 2;
      this.Get<Transform>().Y = 2;
      this.Get<Drawable>().Glyph = '@';
      this.Get<Actor>();
      Inventory = world.GetPrototypeEntity<Inventory>();
    }
  }

  public class Monster : Entity, IPrototype
  {
    public void Init(World world)
    {
      this.Get<Transform>().X = 5;
      this.Get<Transform>().Y = 5;
      this.Get<Drawable>().Glyph = 'M';
      this.Get<Walkable>().Is = false;
    }
  }

  public class Inventory : Entity, IPrototype
  {
    public List<Item> Items = new List<Item>();

    public void Init(World world) { }
  }
}