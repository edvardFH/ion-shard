namespace IonShard.Configuration.Database;

public interface IDatabaseConfigurationService
{
    public MongoDBConfiguration Database { get; }
}
