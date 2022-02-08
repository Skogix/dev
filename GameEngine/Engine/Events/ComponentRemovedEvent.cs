#region
#endregion

namespace ECS {
	public class ComponentRemovedEvent : IEvent {
		public Component Component;
		public Entity Entity;
		public ComponentRemovedEvent(Entity entity, Component component) {
			Entity = entity;
			Component = component;
			Message = $"Removed {component} from {entity.GetHash}";
		}
		public string Message { get; set; }
	}
}