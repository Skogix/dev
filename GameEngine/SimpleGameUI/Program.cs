#region
using System;
using ECS;
#endregion

namespace SimpleGameUI {
	internal class Program {
		private static void Main(string[] args) {
			var w = new World();
			w.AddSystem(new InputSystem(w));
			w.AddSystem(new TileSystem(w, 30, 30));
			w.AddSystem(new DrawSystem(w));
			w.InitSystems();
			
			
			
			var skogix = w.EntityFactory.Get(new PlayerTemplate("Skogix"));

			while (true) {
				w.Run();
			}
		}
	}
}