using Engine;
using Engine.Component;
using Engine.Entity;
using Engine.EntitySystem;
using Engine.Events;
using Engine.Hub;
using Engine.World;
using RogueLikeGame;
using RogueLikeGame.Data;
using RogueLikeGame.Data.Components;
using RogueLikeGame.Helpers;

namespace Console
{
  using System;
  internal static class Program
  {
    private static void Main()
    {
      Console.CursorVisible = false;
      GameWorld w = WorldFactory.CreateWorld<GameWorld>();
      
#if DEBUG
      w.Sub<Debug>(debug => Console.WriteLine(debug.Message));
#endif

      InputSystem inputSystem = w.AddSystem<InputSystem>();
      TileSystem tileSystem = w.AddSystem<TileSystem>(); // borde göras till en prototyp atm,
      GetActionSystem getActionSystem = w.AddSystem<GetActionSystem>();
      DrawSystem drawSystem = w.AddSystem<DrawSystem>();
      tileSystem.Init();
      var skogix = w.GetPrototypeEntity<Skogix>();
      // var monster = w.GetPrototypeEntity<Monster>();
      skogix.Inventory.AddItem(new Items.Sword(3));
      skogix.Inventory.AddItem(new Items.Mace(3));
      foreach (var item in skogix.Inventory.Items)
      {
        Console.WriteLine($"Item: {item}");
      }
      

      // for(int i = 0; i < 10; i++)
      // {
      //   var skogix = w.GetPrototypeEntity<Skogix>(); 
      //   skogix.Inventory.AddItem(new Items.Sword(5));
      // }
      
      while(true)
      {
        w.Run();
        Console.SetCursorPosition(40, 3);
      }
    }
  }
}