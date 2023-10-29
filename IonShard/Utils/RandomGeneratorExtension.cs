namespace IonShard.Utils;

internal static class RandomGeneratorExtension
{
    internal static Guid NextGuid(this Random random)
    {
        var bytes = new byte[16];
        random.NextBytes(bytes);

        return new Guid(bytes);
    }
}
