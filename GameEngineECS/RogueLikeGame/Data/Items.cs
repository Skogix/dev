#region

using Engine.Entity;
using Engine.World;
using RogueLikeGame.Interfaces;

#endregion

namespace RogueLikeGame.Data
{
  public class Items
  {
    public class Mace : IPrototype, IWeapon, Item
    {
      public Mace(int damage)
      {
        Damage = damage;
      }

      public void Init(World world)
      {
        throw new NotImplementedException();
      }

      public int Damage { get; set; }
    }
    public class Sword : IPrototype, IWeapon, Item
    {
      public Sword(int damage)
      {
        Damage = damage;
      }

      public void Init(World world)
      {
        throw new NotImplementedException();
      }

      public int Damage { get; set; }
    }
  }

  public interface IWeapon : Item
  {
    public int Damage { get; set; }
  }
}