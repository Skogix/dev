using RogueLikeGame.Data;
using RogueLikeGame.Interfaces;

namespace RogueLikeGame.Helpers
{
  public static class Extensions
  {
    public static void AddItem(this Inventory inventory, Item item)
    {
      inventory.Items.Add(item);
    }
  }
}