namespace IonShard.Domain.Map.Locations;

public interface IDestination : ILocation
{
    public DateTime EstimatedTimeOfArrival { get; }
}
