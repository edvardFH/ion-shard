namespace IonShard.Models.Ressources;

public class Ressource
{
    public State State { get; }
    public string Name { get; }

    public Ressource(State state, string name)
    {
        State = state;
        Name = name;
    }
}
