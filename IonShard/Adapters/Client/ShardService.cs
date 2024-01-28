using IonShard.Configuration.Wormholes;
using IonShard.Contracts.DTO.Units;
using IonShard.Contracts.DTO.Users;
using IonShard.Domain.Units;
using IonShard.Domain.Users;
using IonShard.Mappers;
using Microsoft.Net.Http.Headers;
using System.Net.Http.Headers;
using System.Text;

namespace IonShard.Adapters.Client;

public class ShardService : IShardService
{
    private readonly HttpClient _httpClient;
    private readonly IWormholesConfigService _wormholesConfigService;

    
    public ShardService(HttpClient httpClient, IWormholesConfigService wormholesConfigService)
    {
        _httpClient = httpClient;

        _wormholesConfigService = wormholesConfigService;
    }

    public async Task<Uri> PutUnitAsync(WormholeConfig wormhole, IUnit unit)
    {
        await PutUserAsync(wormhole, unit.Owner);

        var requestUri = $"/users/{unit.Owner.Id}/units/{unit.Id}";

        using var httpResponseMessage = await _httpClient.PutAsJsonAsync<UnitDTO>(requestUri, unit.ToDTO());
        httpResponseMessage.EnsureSuccessStatusCode();

        unit.Destroy();

        return new Uri(wormhole.BaseUri + requestUri);
    }

    private async Task PutUserAsync(WormholeConfig wormhole, IUser user)
    {
        ConfigureClient(wormhole);

        using var httpResponseMessage = await _httpClient.PutAsJsonAsync<UserDTO>($"/users/{user.Id}", user.ToDTO());
        httpResponseMessage.EnsureSuccessStatusCode();
    }

    private void ConfigureClient(WormholeConfig wormhole)
    {
        _httpClient.BaseAddress = new Uri(wormhole.BaseUri);
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue
            (
                "Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes($"shard-{wormhole.User}:{wormhole.SharedPassword}"))
            );
    }
}
