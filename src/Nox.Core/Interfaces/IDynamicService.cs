﻿using Microsoft.EntityFrameworkCore;

namespace Nox.Core.Interfaces
{
    public interface IDynamicService
    {
        string Name { get; }
        string KeyVaultUri { get; }
        IMetaService MetaService { get; }
        IReadOnlyDictionary<string, IApi>? Apis { get; }
        IReadOnlyDictionary<string, IEntity>? Entities { get; }
        IEnumerable<ILoader>? Loaders { get; }
        Task<bool> ExecuteDataLoaderAsync(ILoader loader);
        Task<bool> ExecuteDataLoadersAsync();
        void AddMetadata(ModelBuilder modelBuilder);
        void SetupRecurringLoaderTasks();
        void EnsureDatabaseCreated(DbContext dbContext);
    }
}