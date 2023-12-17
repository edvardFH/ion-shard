namespace IonShard.Configuration.Authentication;

public record ServerConfig(string BaseUri, string System, string User, string SharedPassword);