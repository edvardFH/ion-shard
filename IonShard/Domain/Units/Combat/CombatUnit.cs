using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Users;
using Shard.Shared.Core;

namespace IonShard.Domain.Units.Combat;

public class CombatUnit : Unit, ICombatUnit
{
    public IReadOnlyList<IWeapon> Weapons { get; }
    public IReadOnlyList<string> CombatPriorities { get; }
    public IReadOnlyDictionary<string, float> DamageReductionMultipliers { get; }
    public bool IsFighting { get; private set; }
    private ITimer _timer;

    public CombatUnit
        (
            string id,
            IUser owner,
            StarSystem system,
            Planet? planet,
            IReadOnlyDictionary<IResource, int> resourceCost,
            string type,
            int healthPoints,
            IEnumerable<IWeapon> weapons,
            IEnumerable<string> combatPriorities,
            IReadOnlyDictionary<string, float> damageReductionMultipliers,
            IClock clock
        )
        : base(id, owner, system, planet, type, resourceCost, healthPoints, clock)
    {
        Weapons = new List<IWeapon>(weapons);
        CombatPriorities = new List<string>(combatPriorities);
        DamageReductionMultipliers = damageReductionMultipliers;
        IsFighting = false;

        _timer = CreateTimer(TimeSpan.FromSeconds(UnitBuildDuration + 1));
    }

    public int ApplyDamage(ICombatUnit damageSource, int damage)
    {
        var damageReceived = (int) (DamageReductionMultipliers[damageSource.Type] * damage);

        if (HealthPoints > damageReceived)
            return (HealthPoints -= damageReceived);

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
        _timer = CreateTimer(GetTimeUntilNextAttack(_clock.Now));
        var choosenTarget = ChooseTarget();

        if (choosenTarget is null)
            return;


        var totalDamage =
            (from weapon in Weapons
             where _clock.Now.Second % weapon.Cooldown == 0
             select weapon.Damage)
             .Sum();


        choosenTarget.ApplyDamage(this, totalDamage);
    }

    private void Destroy()
    {
        Owner.RemoveUnit(this);

        _ = RemoveFromLocationAfterFight();
    }

    private async Task RemoveFromLocationAfterFight()
    {
        await _clock.Delay(500);

        var nearUnits = GetUnitsInLocation();
        nearUnits.Remove(this);
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
            select nextMultiple - _clock.Now
            ).First();
    }

    private ITimer CreateTimer(TimeSpan dueTime) =>
       _clock.CreateTimer(
            Attack,
            this,
            dueTime,
            TimeSpan.FromSeconds(0));
}
