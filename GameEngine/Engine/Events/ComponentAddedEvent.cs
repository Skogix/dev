#region
#endregion

namespace ECS {
	public class ComponentAddedEvent : IEvent {
		public Component Component;
		public Entity Entity;
		public ComponentAddedEvent(Entity entity, Component component) {
			Entity = entity;
			Component = component;
			Message = $"Added {component} to {entity.GetHash}";
		}
		public string Message { get; set; }
	}
}