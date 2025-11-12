using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Hollow264.Common.TestKit.Integration.Controller.Utils;

internal class TestWebAppFactory<TEntryPoint>(Action<IWebHostBuilder>? configureWebHost = null)
    : WebApplicationFactory<TEntryPoint>
    where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        configureWebHost?.Invoke(builder);
    }
}
