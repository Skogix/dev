namespace Engine.Component
{
  public class Component
  {
    internal Component(string ownerHash, string id, object data)
    {
      OwnerHash = ownerHash;
      Id = id;
      Data = data;
    }

    public Component() { }

    internal object Data { get; set; }
    internal string Id { get; set; }

    private string OwnerHash { get; }

    public string Hash => $"{OwnerHash}:{Id}";
  }
}