namespace IonShard.Configuration;

public static class ConfigurationHelper
{
    public static int GetSectionAsInt(this IConfigurationSection section, string key) =>
        section.GetSection(key).Get<int>();

    public static IReadOnlyDictionary<string, int> GetSectionAsIntDictionnary(this IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyDictionary<string, int>>()
            ?? new Dictionary<string, int>();

    public static IReadOnlyDictionary<string, float> GetSectionAsFloatDictionnary(this IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyDictionary<string, float>>()
            ?? new Dictionary<string, float>();

    public static IReadOnlyList<string> GetSectionAsStringList(this IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyList<string>>()
            ?? new List<string>();
}
