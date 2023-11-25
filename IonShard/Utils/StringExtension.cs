namespace IonShard.Utils;

public static class StringExtension
{
    public static string UppercaseFirstWord(this string value)
    {
        if (String.IsNullOrEmpty(value))
            return "";

        value = value.ToLower();
        return Char.ToUpper(value[0]) + value.Substring(1);
    }
}
