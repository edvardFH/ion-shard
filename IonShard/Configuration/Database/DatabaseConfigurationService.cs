namespace IonShard.Configuration.Database;

public class DatabaseConfigurationService : IDatabaseConfigurationService
{
    const string SectionKey = "Database";
    public MongoDBConfiguration Database { get; }

    public DatabaseConfigurationService(IConfiguration configuration)
    {
        Database = configuration
            .GetSection(SectionKey)
            .Get<MongoDBConfiguration>()
            ?? throw new ConfigurationFormatException(SectionKey);
    }
}
