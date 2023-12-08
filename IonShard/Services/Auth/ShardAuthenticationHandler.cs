using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using IonShard.Configuration;
using IonShard.Configuration.Auth;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace IonShard.Services.Auth;

public class ShardAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IReadOnlyDictionary<string, AuthUserEntity> _users;
    
    public ShardAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IConfiguration configuration) : base(options, logger, encoder, clock)
    {
        const string key = "authUsers";
        _users = configuration
            .GetSection(key)
            .Get<IReadOnlyDictionary<string, AuthUserEntity>>() 
                 ?? throw new ConfigurationFormatException(key);
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return AuthenticateResult.Fail("Authorization header is missing");
        
        var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
        var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);

        var user = Authenticate(credentials[0], credentials[1]);
        if (user is null) 
            return AuthenticateResult.Fail("Invalid credentials");
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Basic"));
        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
        
        Context.User = principal;
        
        return AuthenticateResult.Success(ticket);
    }

    private AuthUserEntity? Authenticate(string username, string password)
    {
        return _users.TryGetValue(username, out var user) && user.Password.Equals(password) ? user : null;
    }
}