using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Hollow264.Common.TestKit.Integration.Controller.Utils;

internal class FakeAuthWebAppFactory<TEntryPoint>(Action<IWebHostBuilder>? configuration = null)
    : TestWebAppFactory<TEntryPoint>(configuration)
    where TEntryPoint : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddAuthentication(FakeAuthHandler.SchemeName)
                .AddScheme<AuthenticationSchemeOptions, FakeAuthHandler>(
                    FakeAuthHandler.SchemeName,
                    options => { }
                );
        });
        base.ConfigureWebHost(builder);
    }
}
