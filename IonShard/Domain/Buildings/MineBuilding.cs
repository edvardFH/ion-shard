using IonShard.Domain.Map;
using IonShard.Domain.Map.Resources;
using IonShard.Domain.Units;
using Shard.Shared.Core;

namespace IonShard.Domain.Buildings;

public class MineBuilding : AbstractBuilding, IMineBuilding
{
    private const int MineExtractionPeriode = 60;

    public ResourceCategory ResourceCategory { get; }

    public MineBuilding(IBuilderUnit builder, StarSystem starSystem, Planet planet, ResourceCategory resourceCategory, IClock clock)
        : base(builder, starSystem, planet, "mine")
    {
        ResourceCategory = resourceCategory;
        clock.CreateTimer(
            ExtractOneResource, this,
            TimeSpan.FromSeconds(BuildingBuildDuration + MineExtractionPeriode),
            TimeSpan.FromSeconds(MineExtractionPeriode));
    }

    private void ExtractOneResource(object? state)
    {
        var resourceToExtract = ResourceCategory switch
        {
            ResourceCategory.Solid => GetSolidResourceToExtract(),
            _ => GetMostAbondantResourceOfCategory(ResourceCategory),
        };

        if (resourceToExtract is null || Location.Planet is null)
            return;

        if (!Location.Planet.TakeOneResource(resourceToExtract))
            return;

        Builder.Owner.AddOneResource(resourceToExtract);
    }

    private IResource? GetSolidResourceToExtract()
        => (from resource in Location.Planet?.ResourcesQuantity ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
                where resource.Value > 0
                where resource.Key.Category == ResourceCategory.Solid
                orderby resource.Value descending,
                        resource.Key.Name switch
                        {
                            ResourceName.Titanium => 5,
                            ResourceName.Gold => 4,
                            ResourceName.Aluminium => 3,
                            ResourceName.Iron => 2,
                            ResourceName.Carbon => 1,
                            _ => 0
                        } descending
                select resource.Key).FirstOrDefault();
    
    private IResource? GetMostAbondantResourceOfCategory(ResourceCategory category) => 
        (from resource in Location.Planet?.ResourcesQuantity ?? Enumerable.Empty<KeyValuePair<IResource, int>>()
                where resource.Value > 0
                where resource.Key.Category == category
                orderby resource.Value descending
                select resource.Key).FirstOrDefault();
}
