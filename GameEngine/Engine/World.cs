#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ECS {
	public class World {
		public List<Entity> _allEntities = new List<Entity>();
		public List<Component> _allComponents = new List<Component>();
		public List<ICommand> _allCommands = new List<ICommand>();
		public Dictionary<Entity, ICommand> _commandsByEntity = new Dictionary<Entity, ICommand>();
		public static readonly Dictionary<Type, int> ComponentIdByType = new Dictionary<Type, int>();
		public static readonly List<Type> ComponentTypes = new List<Type>();
		public readonly List<ISystem> _allSystems = new List<ISystem>();
		public readonly List<InitSystem> _initSystems = new List<InitSystem>();
		public readonly List<IRunSystem> _runSystems = new List<IRunSystem>();
		public DebugSystem DebugSystem;
		public EntityFactory EntityFactory;
		public EventSystem EventSystem;
		public World() {
			EventSystem = new EventSystem(this);
			EntityFactory = new EntityFactory(this);
			DebugSystem = new DebugSystem(this);
			EventSystem.Subscribe<ComponentRemovedEvent>(this, OnComponentRemoved);
			EventSystem.Subscribe<ComponentAddedEvent>(this, OnComponentAdded);
			EventSystem.Subscribe<EntityAddedEvent>(this, OnEntityAdded);
			_init();
		}
		private void OnComponentAdded(ComponentAddedEvent e) {
			_allComponents.Add(e.Component);
			if (e.Component is ICommand command) {
				_allCommands.Add(command);
				_commandsByEntity.Add(e.Entity, command);
			}
		}
		private void OnComponentRemoved(ComponentRemovedEvent e) {
			_allComponents.Remove(e.Component);
			if (e.Component is ICommand command) {
				_allCommands.Remove(command);
				_commandsByEntity.Remove(e.Entity);
			}
		}
		private void OnEntityAdded(EntityAddedEvent e) => _allEntities.Add(e.Entity);
		private void _init() {
			var domain = AppDomain.CurrentDomain; // nuvarande domain, dvs inte SkogixEngine utan där den callas
			foreach (var componentType in from assembly in
				                              domain.GetAssemblies() // hämtar loadade assemblies från domainen
			                              from type in assembly.GetTypes() // hämtar typer från assembly
			                              where
				                              type.IsSubclassOf(typeof(Component
				                                                )) // där typen är sealed och ärver av component
			                              select type) {
				var id = ComponentTypes.Count;
				ComponentTypes.Add(componentType);
				ComponentIdByType[componentType] = id;
			}
		}
		/// <summary>
		///     Måste callas innan något annat
		///     Läser in alla components i domain
		/// </summary>
		public void AddSystem(ISystem system) {
			_allSystems.Add(system);
			//if(system is EntitySystem entitySystem) _entitySystems.Add(entitySystem);
			if (system is IRunSystem runSystem) _runSystems.Add(runSystem);
			if (system is InitSystem initSystem) _initSystems.Add(initSystem);
		}
		public void Run() {
			for (var i = 0; i < _allCommands.Count; i++) {
				var command = _allCommands[i];
				command.RunCommand();
				var entity = _commandsByEntity.First(c => c.Value == command).Key;
				entity.RemoveComponent(command.GetType());
			}
			_runSystems.ForEach(s => s.Run());
		}
		public void InitSystems() { _initSystems.ForEach(s => s.Init()); }
		public virtual void Destroy() { }
	}
	public class EngineEvent { }
}