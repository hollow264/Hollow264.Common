using Hollow264.Common.TestKit.Integration.Controller.Utils;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hollow264.Common.TestKit.Integration.Controller;

internal class FakeAuthControllerTestBase<TEntryPoint> : ControllerTestBase<TEntryPoint>
    where TEntryPoint : class
{
    protected override WebApplicationFactory<TEntryPoint> CreateFactory() =>
        new FakeAuthWebAppFactory<TEntryPoint>();

    protected HttpClient GetClient(string? userId = null, string[]? roles = null)
    {
        var client = Sut.CreateClient();

        if (userId is not null)
        {
            client.DefaultRequestHeaders.Add("X-Test-UserId", userId);
            client.DefaultRequestHeaders.Add("X-Test-Authenticated", "true");

            if (roles is { Length: > 0 })
            {
                foreach (var role in roles)
                    client.DefaultRequestHeaders.Add("X-Test-Role", role);
            }
        }

        return client;
    }
}
