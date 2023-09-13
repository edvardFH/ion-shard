namespace IonShard.Models.Map
{
    public class Universe
    {
        private readonly IReadOnlyDictionary<string, StarSystem> _starSystems;

        public IReadOnlyList<StarSystem> Systems
        {
            get => _starSystems.Values.ToList();
        }

        public StarSystem? this[string name]
        {
            get => _starSystems.ContainsKey(name) ? _starSystems[name] : null;
        }


        public Universe(ICollection<StarSystem> starSystems)
        {
            _starSystems = starSystems.ToDictionary(system => system.Name, system => system);
        }
    }
}
