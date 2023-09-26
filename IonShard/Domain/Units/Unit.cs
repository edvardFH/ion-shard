using IonShard.Domain.Map;

namespace IonShard.Domain.Units;

public class Unit
{
    public string Id { get; }
    public string Type => "scout";
    public StarSystem System { get; }
    public Planet? Planet { get; }

    public Unit(string id, StarSystem system, Planet? planet)
    {
        Id = id;
        System = system;
        Planet = planet;
    }

}
