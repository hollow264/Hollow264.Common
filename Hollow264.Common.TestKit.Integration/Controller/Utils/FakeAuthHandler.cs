using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hollow264.Common.TestKit.Integration.Controller.Utils;

public class FakeAuthHandler(
    IOptionsMonitor<AuthenticationSchemeOptions> options,
    ILoggerFactory logger,
    UrlEncoder encoder
) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder)
{
    public const string SchemeName = "FakeAuth";

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var headers = Request.Headers;

        if (!headers.ContainsKey("X-Test-Authenticated"))
            return Task.FromResult(AuthenticateResult.NoResult());

        var userId = headers.TryGetValue("X-Test-UserId", out var userIdHeader)
            ? userIdHeader.ToString()
            : "test-user";

        var roles = headers.TryGetValue("X-Test-Role", out var userRoles)
            ? userRoles.ToArray()
            : ["user"];

        // 构建 claims
        var claims = new List<Claim> { new(ClaimTypes.NameIdentifier, userId) };

        foreach (var role in roles)
            claims.Add(new Claim(ClaimTypes.Role, role ?? "user"));

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
