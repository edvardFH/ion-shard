using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Extensions;

namespace IonShard.Services.Authication;

public class ShardAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly IAuthService _authService;
    
    public ShardAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IAuthService authService) : base(options, logger, encoder, clock)
    {
        _authService = authService;
    }

    protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey(HeaderNames.Authorization))
            return AuthenticateResult.Fail("Authorization header is missing");
        
        var (username, password) = GetCredentialsFromAuthorizationHeader(
            AuthenticationHeaderValue.Parse(Request.Headers[HeaderNames.Authorization]));
        
        var user = _authService.Authenticate(username, password);
        if (user is null) 
            return AuthenticateResult.Fail("Invalid credentials");
        
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationSchemes.Basic.GetDisplayName()));
        var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
        
        Context.User = principal;
        
        return AuthenticateResult.Success(ticket);
    }

    private (string Username, string Password) GetCredentialsFromAuthorizationHeader(AuthenticationHeaderValue headerValue)
    {
        var credentialBytes = Convert.FromBase64String(headerValue.Parameter ?? "");
        var credentials = Encoding.UTF8.GetString(credentialBytes).Split(new[] { ':' }, 2);
        return (Username: credentials.ElementAtOrDefault(0) ?? "",
                Password: credentials.ElementAtOrDefault(1) ?? "");
    }
}