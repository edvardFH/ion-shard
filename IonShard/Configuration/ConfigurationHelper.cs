namespace IonShard.Configuration;

public static class ConfigurationHelper
{
    public static int GetInt(this IConfigurationSection section, string key) =>
        section.GetSection(key).Get<int>();

    public static IReadOnlyDictionary<string, TValue> GetDictionnary<TValue>(this IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyDictionary<string, TValue>>()
            ?? new Dictionary<string, TValue>();

    public static IReadOnlyList<TValue> GetList<TValue>(this IConfigurationSection section, string key) =>
        section.GetSection(key)
            .Get<IReadOnlyList<TValue>>()
            ?? new List<TValue>();
}
