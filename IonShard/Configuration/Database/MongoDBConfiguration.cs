namespace IonShard.Configuration.Database;

public record MongoDBConfiguration(string Database, string Host, int Port, string User, string Password)
{
    public string ConnectionString => $@"mongodb://{User}:{Password}@{Host}:{Port}";
}
