﻿using Nox.Dynamic.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nox.Dynamic.MetaData;

public sealed class ServiceDatabase : DatabaseBase, IServiceDatabase  {}

internal class ServiceDatabaseValidator : DatabaseValidator
{
    public ServiceDatabaseValidator(ServiceValidationInfo info) : base(info) { }
}