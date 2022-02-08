#region

using Engine.Component;

#endregion

namespace RogueLikeGame.Data.Components
{
  public enum GameAction
  {
    None,
    Move,
    Attack,
  }

  public class Player : Component { }

  public class Actor : Component
  {
    public GameAction action;
    public EDirection direction = EDirection.Right;
  }

  public class Health : Component
  {
    public int current;
    public int max;
  }

  public class Attack : Component
  {
    public int damage;
    public int hitChance;
  }

  public class Defense : Component
  {
    public int armor;
    public int dodgeChance;
  }
}