#region
using System;
#endregion

namespace ECS {
	public abstract class Component : ICloneable {
		public object Clone() { return MemberwiseClone(); }
		public override string ToString() { return $"[{GetType().Name}]"; }
	}
}