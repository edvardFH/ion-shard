namespace IonShard.Models.Space
{
    public class Universe
    {
        private readonly Dictionary<string, StarSystem> _starSystems;

        public StarSystem this[string name]
        {
            get => _starSystems[name];
        }


        public Universe(ICollection<StarSystem> starSystems)
        {
            _starSystems = starSystems.ToDictionary(system => system.Name, system => system);
        }


        public List<StarSystem> GetAllSystems() => _starSystems.Values.ToList();
    }
}
