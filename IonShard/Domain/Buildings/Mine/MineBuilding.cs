using IonShard.Configuration.Gamerules;
using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units.Builder;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings.Mine;

public class MineBuilding : Building, IMineBuilding
{
    private const int MineExtractionPeriode = 60;

    public ResourceCategory ResourceCategory { get; }
    private readonly ITimer _miningTimer;


    public MineBuilding
        (
            IBuilderUnit builder,
            StarSystem starSystem,
            Planet planet,
            ResourceCategory resourceCategory,
            IClock clock,
            DateTime? estimatedBuildTime = null,
            bool isBuilt = false
        )
        : base(builder, starSystem, planet, "mine", estimatedBuildTime, isBuilt)
    {
        ResourceCategory = resourceCategory;
        _miningTimer = clock.CreateTimer(
            ExtractOneResource,
            this,
            TimeSpan.FromSeconds(BuildingBuildDuration + MineExtractionPeriode),
            TimeSpan.FromSeconds(MineExtractionPeriode));
    }

    private void ExtractOneResource(object? state)
    {
        var resourceToExtract = ResourceCategory switch
        {
            ResourceCategory.Solid => GetSolidResourceToExtract(),
            _ => GetMostAbondantResourceOfCategoryToExtract(ResourceCategory),
        };

        if (resourceToExtract is null || Location.Planet is null)
            return;

        if (!Location.Planet.TakeOneResource(resourceToExtract))
            return;

        Builder.Owner.AddResource(resourceToExtract, 1);
    }

    private IResource? GetSolidResourceToExtract() =>
        (
            from resourceQuantity in Location.Planet?.ResourcesQuantity
                ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
             where resourceQuantity.Value > 0
             where resourceQuantity.Key.Category == ResourceCategory.Solid
             orderby resourceQuantity.Value descending,
                     resourceQuantity.Key.Rarity descending
             select resourceQuantity.Key
         )
         .FirstOrDefault();

    private IResource? GetMostAbondantResourceOfCategoryToExtract(ResourceCategory category) =>
        (
            from resourceQuantity in Location.Planet?.ResourcesQuantity
             ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
             where resourceQuantity.Value > 0
             where resourceQuantity.Key.Category == category
             orderby resourceQuantity.Value descending
             select resourceQuantity.Key
         )
        .FirstOrDefault();
}
