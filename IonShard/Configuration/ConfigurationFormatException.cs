namespace IonShard.Configuration;

public class ConfigurationFormatException : FormatException
{
    public ConfigurationFormatException(string key)
        : base($"Invalid configuration format, {key} could not be found.")
    {
    }
}
