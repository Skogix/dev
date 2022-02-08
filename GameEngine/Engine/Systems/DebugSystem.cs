#region
using System;
#endregion

namespace ECS {
	public class DebugSystem {
		public DebugSystem(World world) {
			World = world; 
		}
		public World World { get; }
		public void Debug(string message) {
#if DEBUG
			Console.WriteLine($"DEBUG::: {message}");
#endif
		}
	}
}