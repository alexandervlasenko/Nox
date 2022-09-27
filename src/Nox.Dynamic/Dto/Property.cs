﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nox.Dynamic.Dto
{
    public class Property
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsPrimaryKey { get; set; } = false;
        public bool IsAutoNumber { get; set; } = false;
        public bool IsForeignKey { get; set; } = false;
        public bool IsRequired { get; set; } = false;
        public bool IsUnicode { get; set; } = true;
        public bool CanFilter { get; set; } = false;
        public bool CanSort { get; set; } = false;
        public int MinWidth { get; set; } = 0;
        public int MaxWidth { get; set; } = 512;
        public int MinValue { get; set; } = int.MinValue;
        public int MaxValue { get; set; } = int.MaxValue;
        public object Default { get; set; } = string.Empty;

        public Type NetDataType()
        {
            var propType = Type?.ToLower() ?? "string";

            return propType switch
            {
                "string" => typeof(String),
                "varchar" => typeof(String),
                "nvarchar" => typeof(String),
                "char" => typeof(String),
                "url" => typeof(String),
                "email" => typeof(String),
                "date" => typeof(DateOnly),
                "time" => typeof(TimeOnly),
                "timespan" => typeof(TimeSpan),
                "datetime" => typeof(DateTime),
                "bool" => typeof(Boolean),
                "boolean" => typeof(Boolean),
                "object" => typeof(Object),
                "int" => typeof(Int32),
                "uint" => typeof(UInt32),
                "tinyint" => typeof(Int16),
                "bigint" => typeof(Int64),
                "money" => typeof(Decimal),
                "smallmoney" => typeof(Decimal),
                "decimal" => typeof(Decimal),
                "real" => typeof(Single),
                "float" => typeof(Single),
                "bigreal" => typeof(Double),
                "bigfloat" => typeof(Double),
                _ => typeof(String)
            };

        }


    }
}