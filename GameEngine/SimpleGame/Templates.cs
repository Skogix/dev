#region
using System;
using System.Collections.Generic;
using ECS;
#endregion

namespace SimpleGameUI {
	public class PlayerTemplate : ITemplate {
		public PlayerTemplate(string name) {
			Components = new List<Component> {
				new Health(100, 100), 
				new NameComponent(name),
				new Actor(),
				new Transform(4, 4),
				new DrawableComponent(ConsoleColor.Green, ConsoleColor.Black, '@'),
			};
		}
		public List<Component> Components { get; set; }
	}
	public class NameComponent : Component {
		public NameComponent(string name) { Name = name; }
		public string Name { get; set; }
	}
	public class MonsterTemplate : ITemplate {
		public MonsterTemplate(string name) {
			Components = new List<Component> {
				new Health(25, 25),
				new NameComponent(name),
			};
		}
		public List<Component> Components { get; set; }
	}
	public class TileTemplate : ITemplate {
		public List<Component> Components { get; set; }
		public TileTemplate(int x, int y) {
			Components = new List<Component>();
			Components.Add(new NameComponent("Wall"));
			Components.Add(new DrawableComponent(ConsoleColor.White, ConsoleColor.Black, '#'));
			Components.Add(new Transform(x, y));
		}
	}
	public class DrawableComponent : Component {
		public ConsoleColor Foreground { get; set; }
		public ConsoleColor Background { get; set; }
		public char Glyph { get; set; }
		public DrawableComponent(ConsoleColor foreground, ConsoleColor background, char glyph) {
			Foreground = foreground;
			Background = background;
			Glyph = glyph;
		}
	}
}