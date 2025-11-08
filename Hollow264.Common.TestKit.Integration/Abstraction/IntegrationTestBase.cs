using NUnit.Framework;

namespace Hollow.Common.TestKit.Integration.Abstraction;

[TestFixture]
public abstract class IntegrationTestBase<T>
    where T : class
{
    public T Sut { get; set; } = default!;
}
