using System.Collections.Generic;
using System.IO;

namespace IonShard.UnitTests.Repository;

public class LocalTestRepository
{
    private const string JsonFilePathName = "../../../TestResources/TestRepoSystems.json";
    private static LocalTestRepository _instance;
    private Universe _universe;
    private BuildingRepository _buildingRepository;
    private UserRepository _userRepository;

    private LocalTestRepository(List<StarSystem> systems)
    {
        _universe = new Universe(systems);
        _buildingRepository = new BuildingRepository();
        _userRepository = new UserRepository();
    }

    public StarSystem? this[string systemName] => _universe[systemName];

    public static LocalTestRepository GetInstance()
    {
        if (_instance is null)
        {
            string jsonString = File.ReadAllText(JsonFilePathName);
            List<StarSystem> systems = JsonSerializer.Deserialize<List<StarSystem>>(jsonString)!;
            _instance = new LocalTestRepository(systems);
        }

        return _instance;
    }
}