using IonShard.Models.Map;

namespace IonShard.Models.Units;

public class Unit
{
    public string Id { get; }
    public string Type => "scout";
    public StarSystem System { get; }
    public Planet Planet { get; }
}
