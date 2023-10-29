namespace IonShard.Contracts.DTO.Map;

public record StarSystemDTO(string name, IReadOnlyList<PlanetDTO> Planets);
