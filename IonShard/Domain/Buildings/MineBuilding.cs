using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class MineBuilding : AbstractBuilding, IMineBuilding
{
    private const int MineExtractionPeriode = 60;

    public ResourceCategory ResourceCategory { get; }

    public MineBuilding(
        IBuilderUnit builder,
        StarSystem starSystem,
        Planet planet,
        ResourceCategory resourceCategory,
        IClock clock,
        DateTime? estimatedBuildTime = null,
        bool isBuilt = false)
        : base(builder, starSystem, planet, "mine", estimatedBuildTime, isBuilt)
    {
        ResourceCategory = resourceCategory;
        clock.CreateTimer(
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

        Builder.Owner.AddOneResource(resourceToExtract);
    }

    private IResource? GetSolidResourceToExtract() =>
        (from resourceQuantity in Location.Planet?.ResourcesQuantity
            ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
         where resourceQuantity.Value > 0
         where resourceQuantity.Key.Category == ResourceCategory.Solid
         orderby resourceQuantity.Value descending,
                 resourceQuantity.Key.Name switch
                 {
                     ResourceName.Titanium => 5,
                     ResourceName.Gold => 4,
                     ResourceName.Aluminium => 3,
                     ResourceName.Iron => 2,
                     ResourceName.Carbon => 1,
                     _ => 0
                 } descending
         select resourceQuantity.Key)
         .FirstOrDefault();

    private IResource? GetMostAbondantResourceOfCategoryToExtract(ResourceCategory category) =>
        (from resourceQuantity in Location.Planet?.ResourcesQuantity
         ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
         where resourceQuantity.Value > 0
         where resourceQuantity.Key.Category == category
         orderby resourceQuantity.Value descending
         select resourceQuantity.Key)
        .FirstOrDefault();
}
