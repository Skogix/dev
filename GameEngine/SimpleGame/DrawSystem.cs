using System;
using ECS;

namespace SimpleGameUI {
	public class DrawSystem : EntitySystem, IRunSystem, InitSystem {
		public DrawSystem(World world) : base(world) {
			
		}
		public void Run() {
			foreach (var entity in Entities) {
				var drawable = entity.GetComponent<DrawableComponent>();
				var transform = entity.GetComponent<Transform>();
				Console.SetCursorPosition(transform.X, transform.Y);
				Console.Write(drawable.Glyph);
			}
		}
		public void Init() {
			AddFilter(typeof(DrawableComponent));
			AddFilter(typeof(Transform));
			Console.CursorVisible = false;
		}
	}
}