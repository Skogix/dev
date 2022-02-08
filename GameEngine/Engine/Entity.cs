#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ECS {
	public class Entity {
		private readonly int _gen;
		private readonly int _id;
		public readonly Dictionary<Type, Component> ComponentsByType;
		protected internal Entity(World world, int id) {
			_id = id;
			_gen = 0;
			ComponentsByType = new Dictionary<Type, Component>();
			World = world;
		}
		public string GetHash => $"{_id}-{_gen}";
		public World World { get; }
		public string GetInfo => $"Hash: {GetHash} \nComponents ({ComponentsByType.Count})\nName: {GetType().Name}";
		
		internal bool ContainsComponent(Type componentType) { return ComponentsByType.ContainsKey(componentType); }
		internal bool ContainsComponent(IEnumerable<Type> componentTypes) {
			return componentTypes.All(ComponentsByType.ContainsKey);
		}
		internal bool ContainsComponent(params Type[] componentTypes) {
			return ContainsComponent(componentTypes as IEnumerable<Type>);
		}
		public void AddComponent(Component component) {
			var componentType = component.GetType();
			ComponentsByType[componentType] = component;
			World.EventSystem.Publish(new ComponentAddedEvent(this, component));
		}
		internal void AddComponents(IEnumerable<Component> components) { components.ToList().ForEach(AddComponent); }
		public T GetComponent<T>() where T : Component { return ComponentsByType[typeof(T)] as T; }
		
		internal void RemoveComponent(Component component) { RemoveComponent(component.GetType()); }
		internal void RemoveComponent<T>() { RemoveComponent(typeof(T)); }
		internal void RemoveComponent(Type componentType) {
			var component = ComponentsByType.Where(c => c.Key == componentType).First().Value;
			World.EventSystem.Publish(new ComponentRemovedEvent(this, component));
			ComponentsByType.Remove(componentType);
		}
	}
}