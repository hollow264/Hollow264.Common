using Hollow.Common.TestKit.Integration.Abstraction;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace Hollow.Common.TestKit.Integration.EFCore;

/// <summary>
/// 提供针对 EF Core 仓储类的集成测试基础设施。
/// 封装 DbContext 生命周期、数据库初始化与资源清理逻辑。
/// </summary>
/// <typeparam name="TRepository">被测仓储类型</typeparam>
/// <typeparam name="TDbContext">数据库上下文类型</typeparam>
public abstract class EFCoreRepositoryTestBase<TRepository, TDbContext>
    : IntegrationTestBase<TRepository>
    where TRepository : class
    where TDbContext : DbContext
{
    /// <summary>
    /// 当前测试使用的数据库上下文实例。
    /// </summary>
    protected TDbContext DbContext { get; private set; } = default!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        using var context = CreateDbContext();
        CreateDatabase(context);
    }

    [SetUp]
    public void SetUp()
    {
        DbContext = CreateDbContext();
        Sut = CreateRepository(DbContext);
        Assert.That(DbContext, Is.Not.Null, "DbContext is not null.");
        Assert.That(Sut, Is.Not.Null, "Repository Sut is not.");
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            CleanupDatabase();
        }
        finally
        {
            DisposeResources();
        }
    }

    /// <summary>创建仓储实例。</summary>
    protected abstract TRepository CreateRepository(TDbContext dbContext);

    /// <summary>创建数据库上下文实例。</summary>
    protected abstract TDbContext CreateDbContext();

    /// <summary>创建新数据库。</summary>
    protected virtual void CreateDatabase(TDbContext dbContext)
    {
        dbContext.Database.EnsureCreated();
    }

    /// <summary>清理数据库内容。</summary>
    protected virtual void CleanupDatabase() { }

    /// <summary>释放额外资源（可在子类中重写）。</summary>
    protected virtual void DisposeResources()
    {
        DbContext.Dispose();
    }
}
