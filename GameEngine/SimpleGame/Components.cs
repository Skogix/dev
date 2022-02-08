#region
using ECS;
#endregion

namespace SimpleGameUI {
	public class Health : Component {
		public Health(int health, int maxHealth) {
			Hp = health;
			MaxHp = maxHealth;
		}
		public int Hp { get; set; }
		public int MaxHp { get; set; }
	}
	public class Actor : Component {
		public char Key { get; set; }
	}
	public class Transform : Component {
		public int X { get; set; }
		public int Y { get; set; }
		public Transform(int x, int y) {
			X = x;
			Y = y;
		}
	}
	
	
	
	
}