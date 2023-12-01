namespace IonShard.Domain.Units.Combat;

public interface IWeapon
{
    string Name { get; }
    int Damage { get; }
    int Cooldown { get; }
}
