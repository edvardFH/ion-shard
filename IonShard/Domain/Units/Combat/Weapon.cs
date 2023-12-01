namespace IonShard.Domain.Units.Combat;

public record Weapon(string Name, int Damage, int Cooldown) : IWeapon
{
}
