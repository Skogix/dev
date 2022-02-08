#region
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ECS {
	public class EventSystem {
		private readonly List<Handler> Handlers = new List<Handler>();
		public EventSystem(World world) { World = world; }
		public World World { get; }
		public void Subscribe<T>(object sub, Action<T> handler) { Handlers.Add(GetHandler<T>(sub, handler)); }
		public void Publish<T>(object sender, T data) {
			if (data is IMessage iMessage) World.DebugSystem.Debug(iMessage.Message);
			foreach (var handler in Handlers.Where(h => h.Type == typeof(T)))
				if (handler.Action is Action<T> sendAction)
					sendAction(data);
		}
		public void Publish<T>(T data) { Publish(null, data); }
		private static Handler GetHandler<T>(object sub, Delegate handler) {
			return new Handler {Action = handler, Type = typeof(T), Sender = new WeakReference(sub)};
		}
		private class Handler {
			public Delegate Action { get; set; }
			public Type Type { get; set; }
			public object Sender { get; set; }
		}
	}
}