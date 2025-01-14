﻿using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;

namespace Nox.Core.Interfaces.Database;

public interface IDynamicModel
{
    ModelBuilder ConfigureDbContextModel(ModelBuilder modelBuilder);
    IDataProvider GetDatabaseProvider();
    IQueryable GetDynamicCollection(DbContext context, string dbSetName);
    object GetDynamicNavigation(DbContext context, string dbSetName, object id, string navName);
    object GetDynamicObjectProperty(DbContext context, string dbSetName, object id, string propName);
    object GetDynamicSingleResult(DbContext context, string dbSetName, object id);
    IEdmModel GetEdmModel();
    object PostDynamicObject(DbContext context, string dbSetName, string json);
    object PutDynamicObject(DbContext context, string dbSetName, string json);
    object PatchDynamicObject(DbContext context, string dbSetName, object id, string json);
    void DeleteDynamicObject(DbContext context, string dbSetName, object id);
}
