namespace IonShard.Configuration.Database;

public class ServerConfiguration
{
    public MongoDBConfiguration MongoDB { get; }

    public ServerConfiguration(MongoDBConfiguration mongoDB)
    {
        MongoDB = mongoDB;
    }
}
