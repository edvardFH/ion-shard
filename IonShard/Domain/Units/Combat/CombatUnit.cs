using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Combat;

public class CombatUnit : Unit, ICombatUnit
{
    public int HealthPoints { get; private set; }
    public IReadOnlyList<IWeapon> Weapons { get; }
    public IReadOnlyList<string> CombatPriorities { get; }
    private ITimer _timer;

    public CombatUnit
        (
            IUser owner,
            StarSystem system,
            Planet? planet,
            IReadOnlyDictionary<Resource, int> resourceCost,
            string type,
            int healthPoint,
            IEnumerable<IWeapon> weapons,
            IEnumerable<string> combatPriorities,
            IClock clock
        )
        : base(owner, system, planet, type, resourceCost, clock)
    {
        HealthPoints = healthPoint;
        Weapons = new List<IWeapon>(weapons);
        CombatPriorities = new List<string>(combatPriorities);


        _timer = _clock.CreateTimer(
            Attack,
            this,
            TimeSpan.FromSeconds(UnitBuildDuration),
            GetTimeUntilNextAttack(_clock.Now.AddSeconds(UnitBuildDuration)));
    }

    public int ApplyDamage(int damage)
    {
        if (HealthPoints >= damage)
            return (HealthPoints -= damage);

        Destroy();

        return (HealthPoints = 0);
    }

    private ICombatUnit? ChooseTarget()
    {
        var nearUnits = GetUnitsInLocation();

        if ((from unit in nearUnits
             where unit.Owner != this.Owner
             where unit is ICombatUnit
             orderby CombatPriorities.ToList().IndexOf(unit.Type)
             select unit)
            .FirstOrDefault() is ICombatUnit combatUnit)
            return combatUnit;

        return null;
    }


    private void Attack(object? state)
    {
        var choosenTarget = ChooseTarget();

        if (choosenTarget is null)
            return;

        (from weapon in Weapons
         where _clock.Now.Second % weapon.Cooldown == 0
         select weapon)
         .ToList()
         .ForEach(weapon => choosenTarget.ApplyDamage(weapon.Damage));

        _timer.Change(GetTimeUntilNextAttack(_clock.Now), TimeSpan.MaxValue);
    }

    private void Destroy()
    {
        var nearUnits = GetUnitsInLocation();

        nearUnits.Remove(this);
        Owner.RemoveUnit(this);
    }

    private IList<IUnit> GetUnitsInLocation() => Location.Planet is null
            ? Location.System.Units
            : Location.Planet.Units;

    private TimeSpan GetTimeUntilNextAttack(DateTime dateTime)
    {
        return (
            from weapon in Weapons
            let sec = weapon.Cooldown
            let delta = sec - (dateTime.Second % sec)
            let nextMultiple = dateTime.AddSeconds(delta)
            orderby (nextMultiple - dateTime).TotalSeconds
            select nextMultiple - DateTime.Now
            ).First();
    }

    private bool DoesAttackAt(DateTime dateTime) =>
        (from weapon in Weapons
         where dateTime.Second % weapon.Cooldown == 0
         select weapon)
        .Any();
}
