# Hollow.Common.TestKit.Integration

> **通用 EF Core & NUnit 仓储集成测试基础类库**

## 简介

`Hollow.Common.TestKit.Integration` 提供了一套可复用的集成测试基础设施，用于：

- EF Core 仓储类集成测试
- 控制器或其他服务的集成测试
- 封装 DbContext 生命周期、数据库初始化与清理逻辑
- 支持多种 ORM 和数据库 Provider 的扩展

目标是让团队可以快速搭建高质量、隔离性好且性能优化的集成测试。

---

## 功能特性

- **通用集成测试基类**：`IntegrationTestBase<T>`
- **EF Core 仓储测试基类**：`EFCoreRepositoryTestBase<TRepository, TDbContext>`

  - 自动管理 DbContext 生命周期
  - 数据库只初始化一次，减少重复建表开销
  - 支持虚方法扩展，如 `CleanupDatabase()`、`DisposeResources()`

- **NUnit 友好**：支持 `[SetUp]`、`[TearDown]`、`[OneTimeSetUp]` 等生命周期管理
- **可扩展性**：可轻松添加 Dapper、Controller 或其他 ORM/服务测试基类
- **NuGet 打包**：方便在多个项目中复用

---

## 安装

### NuGet

```bash
dotnet add package Hollow.Common.TestKit.Integration
```

---

## 快速开始

### 1. 创建仓储测试基类

```csharp
public class UserRepositoryTests
    : EFCoreRepositoryTestBase<UserRepository, AppDbContext>
{
    protected override AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase($"TestDb_{Guid.NewGuid()}")
            .Options;
        return new AppDbContext(options);
    }

    protected override UserRepository CreateRepository(AppDbContext dbContext)
    {
        return new UserRepository(dbContext);
    }
}
```

## 扩展指南

- **自定义数据库清理**
  子类可覆盖 `CleanupDatabase()`，实现事务回滚或表清空：

```csharp
protected override void CleanupDatabase()
{
    DbContext.Users.RemoveRange(DbContext.Users);
    DbContext.SaveChanges();
}
```

- **释放额外资源**
  覆盖 `DisposeResources()` 释放其他测试资源：

```csharp
protected override void DisposeResources()
{
    base.DisposeResources();
    // 其他清理逻辑
}
```
