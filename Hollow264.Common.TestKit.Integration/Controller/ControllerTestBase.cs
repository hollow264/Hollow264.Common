using Microsoft.AspNetCore.Mvc.Testing;
using NUnit.Framework;

namespace Hollow264.Common.TestKit.Integration.Controller;

public class ControllerTestBase<TEntryPoint>
    where TEntryPoint : class
{
    protected WebApplicationFactory<TEntryPoint> Sut = null!;

    [OneTimeSetUp]
    public void InitWebAppFactory()
    {
        Sut = CreateFactory();
    }

    protected virtual WebApplicationFactory<TEntryPoint> CreateFactory() => new();

    [OneTimeTearDown]
    public void Dispose()
    {
        Sut.Dispose();
    }
}
