using System.Text.Json.Serialization;

namespace IonShard.Configuration.Database;

public record MongoDBConfiguration(string DatabaseName, string Host, int Port, string User, string Password)
{
    [JsonIgnore]
    public string ConnectionString => $@"mongodb://{User}:{Password}@{Host}:{Port}";
}
