using Shard.Shared.Core;

namespace IonShard.Domain.Units.Combat;

public interface ICombatUnit : IUnit
{
    public IReadOnlyList<IWeapon> Weapons { get; }
    public IReadOnlyList<string> CombatPriorities { get; }
    public IReadOnlyDictionary<string, float> DamageReductionMultipliers { get; }
    public bool IsFighting { get; }
    public int ApplyDamage(ICombatUnit damageSource, int damage);
}
