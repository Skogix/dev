namespace ECS {
	public class EntityAddedEvent : IEvent {
		public readonly Entity Entity;
		public EntityAddedEvent(Entity entity) {
			Entity = entity;
			Message = $"Created {entity.GetHash}";
		}
		public string Message { get; set; }
	}
}