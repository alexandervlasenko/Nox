﻿namespace Nox.Core.Interfaces
{
    public interface IEtlExecutor
    {
        Task<bool> ExecuteAsync(IMetaService service);
        Task<bool> ExecuteLoaderAsync(IMetaService service, ILoader loader, IEntity entity);
    }
}