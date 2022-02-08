#region

#endregion

namespace Engine.Hub
{
  public class Hub
  {
    private readonly List<object> _bus = new List<object>();
    private readonly List<Handler> _handlers = new List<Handler>();

    // ReSharper disable once NotAccessedField.Local
    private World.World _world;

    public Hub(World.World world)
    {
      _world = world;
    }

    // default så vi slipper nullproblem, kolla snyggare lösning
    internal void Publish<T>(T data = default)
    {
      // för varje handler
      foreach(var handler in _handlers.Where(handler => handler.Type == typeof(T)))
        // som skickar samma action
        if(handler.Action is Action<T> sendAction)
          // skicka data
          sendAction(data);
      // ToDo: försök jävlas med func igen
    }

    // action är ett "färdigt" delegat utan return och ingen multicasting / += 
    // så en func utan return
    internal void Subscribe<T>(object sub, Action<T> handler)
    {
      var h = GetHandler<T>(sub, handler);
      _handlers.Add(h);
    }

    // skapa handlern och adda
    private Handler GetHandler<T>(object sub, Delegate handler)
    {
      var output = new Handler
      {
        Action = handler, // faktiska delegatet
        Type = typeof(T),
        Sender = new WeakReference(sub), // håller en referens till sender, både för referens och GC
      };
      return output;
    }

    internal void UnSubscribe<T>(object sub, Action<T> handler = null)
    {
      var handlers = _handlers.Where(h =>
                                       h.Sender.Target != null && (h.Sender.IsAlive ||
                                                                   h.Sender.Target.Equals(sub)) && h.Type == typeof(T))
                              .ToList();
      foreach(var h in handlers) _handlers.Remove(h);
    }

    internal void Push<T>(T data) where T : new()
    {
      // dat boxing though
      _bus.Add(data ?? new T());
    }

    internal IEnumerable<T> Pull<T>(Type type = default)
    {
      var output = type != null ? _bus.Where(o => o.GetType() == type) : _bus.Where(o => o.GetType() == typeof(T));
      foreach(var o in output) yield return (T) o;
    }
  }

  // försök hålla allt generic för kommer säkert vilja kunna ha callbacks och liknande
  internal class Handler
  {
    public Delegate Action { get; set; }
    public Type Type { get; set; }
    public WeakReference Sender { get; set; }
  }
}