#region
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ECS {
	public class EntityFactory {
		private static int _idCount;
		public EntityFactory(World world) { W = world; }
		public World W { get; }
		private static int Next() { return _idCount++; }
		private Entity NewEntity() {
			var e = new Entity(W, Next());
			W.EventSystem.Publish(new EntityAddedEvent(e));
			return e;
		}
		private List<Component> CloneComponents(Entity sourceEntity) {
			return sourceEntity.ComponentsByType.Values.Select(c => c.Clone() as Component).ToList();
		}
		
		public Entity Get() { return NewEntity(); }
		public Entity Get(Component component) {
			var e = NewEntity();
			e.AddComponent(component);
			return e;
		}
		public Entity Get(Entity sourceEntity) {
			var e = NewEntity();
			CloneComponents(sourceEntity).ForEach(e.AddComponent);
			return e;
		}
		public Entity Get(IEnumerable<Component> components) {
			var e = NewEntity();
			components.ToList().ForEach(e.AddComponent);
			return e;
		}
		public Entity Get(ITemplate template) {
			var e = NewEntity();
			template.Components.ToList().ForEach(e.AddComponent);
			return e;
		}
		public Entity Get(params Component[] components) {
			var e = NewEntity();
			components.ToList().ForEach(e.AddComponent);
			return e;
		}
	}
}