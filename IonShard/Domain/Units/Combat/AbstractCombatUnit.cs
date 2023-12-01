using IonShard.Domain.Map;
using IonShard.Domain.Users;

namespace IonShard.Domain.Units.Combat;

public abstract class AbstractCombatUnit : AbstractUnit, ICombatUnit
{
    public int HealthPoints { get; private set;  }
    public IReadOnlyList<IWeapon> Weapons { get; }
    public IReadOnlyList<string> CombatPriorities { get; }
    
    public AbstractCombatUnit(
        IUser owner,
        StarSystem system,
        Planet? planet,
        string type,
        int healthPoint,
        IEnumerable<IWeapon> weapons,
        IEnumerable<string> combatPriorities)
        : base(owner, system, planet, type)
    {
        HealthPoints = healthPoint;
        Weapons = new List<IWeapon>(weapons);
        CombatPriorities = new List<string>(combatPriorities);
    }

    public int ApplyDamage(int damage)
    {
        if(HealthPoints >= damage)
            return (HealthPoints -= damage);

        return (HealthPoints = 0);
    }

    protected abstract IUnit ChooseTarget();
}
