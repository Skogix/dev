using System;
using System.Collections.Generic;
using System.Linq;

namespace ECS {
	public abstract class CommandSystem : ISystem{
		public World World { get; }
		private readonly List<Type> _filters;
		protected readonly List<Type> Commands;

		public CommandSystem(World world) {
			Commands = new List<Type>();
			_filters = new List<Type>();
			World = world; 
			World.EventSystem.Subscribe<ComponentAddedEvent>(this, OnComponentAdded);
			World.EventSystem.Subscribe<ComponentRemovedEvent>(this, OnComponentRemoved);
		}
		protected CommandSystem(World world, params Type[] commandTypes) : this(world) {
			commandTypes.ToList().ForEach(AddFilter);
		}
		internal void AddFilter(Type componentType) {
			if (_filters.Contains(componentType) == false) _filters.Add(componentType);
		}		
		protected void AddFilter<T>() where T: ICommand{
			AddFilter(typeof(T));
		}
		private bool EntityHasCommand(Entity entity, ICommand command) {
			return entity.ContainsComponent(command.GetType());
		}
		private void OnComponentRemoved(ComponentRemovedEvent e) {
			
		}
		private void OnComponentAdded(ComponentAddedEvent e) {
			
		}
		
	}
	public interface ISystem { }
	public interface ICommand {
		public void RunCommand();
	}
}