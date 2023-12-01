using Shard.Shared.Core;

namespace IonShard.Domain.Units.Combat;

public interface ICombatUnit : IUnit
{
    public int HealthPoints { get; }
    public IReadOnlyList<IWeapon> Weapons { get; }
    public IReadOnlyList<string> CombatPriorities { get; }
    public int ApplyDamage(int damage);
}
