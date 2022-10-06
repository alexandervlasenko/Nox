﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization;

namespace SourceGenerator
{
    [Generator]
    public class DynamicGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {

#if DEBUG
            if (false && !Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif 
            var assemblyName = context.Compilation.AssemblyName;
            
            
            var mainSyntaxTree = context.Compilation.SyntaxTrees
                .FirstOrDefault(x => x.FilePath.EndsWith("Program.cs"));

            if (mainSyntaxTree == null) return;

            
            var programPath = Path.GetDirectoryName(mainSyntaxTree.FilePath);

            var settingsFile = Path.Combine(programPath, "appsettings.json");
            
            if (!File.Exists(settingsFile)) return;


            var config = JObject.Parse(File.ReadAllText(settingsFile));

            var definitionRootPath = config["Nox"]["DefinitionRootPath"].ToString();

            var definitionRootFullPath = Path.GetFullPath(Path.Combine(programPath, definitionRootPath));

            if (!Directory.Exists(definitionRootFullPath)) return;


            var deserializer = new DeserializerBuilder().Build();

            var entities = Directory
                .EnumerateFiles(definitionRootFullPath, "*.entity.nox.yaml", SearchOption.AllDirectories)
                .Select(f => deserializer.Deserialize(new StringReader(File.ReadAllText(f))))
                .ToList();


            foreach (Dictionary<object,object> entity in entities)
            {
                var sb = new StringBuilder();

                sb.AppendLine($@"// autogenerated");
                sb.AppendLine($@"using Nox.Dynamic.Models;");
                sb.AppendLine($@"");
                sb.AppendLine($@"namespace {assemblyName}.Nox;");
                sb.AppendLine($@"");
                sb.AppendLine($@"public class {entity["Name"]} : IDynamicEntity");
                sb.AppendLine($@"{{");

                var attributes = (List<object>)entity["Attributes"];
                foreach (Dictionary<object,object> attr in attributes)
                {
                    sb.AppendLine($@"   public {ClassDataType((string)attr["Type"])} {attr["Name"]} {{get; set;}}");
                }
                sb.AppendLine($@"}}");

                var hintName = $"{entity["Name"]}.g.cs";
                var source = SourceText.From(sb.ToString(), Encoding.UTF8);

                context.AddSource(hintName, source);
                
            }

        }

        public void Initialize(GeneratorInitializationContext context)
        {
        }

        public string ClassDataType(string type)
        {
            var propType = type.ToLower() ?? "string";

            return propType switch
            {
                "string" => "string",
                "varchar" => "string",
                "nvarchar" => "string",
                "char" => "string",
                "guid" => "guid",
                "url" => "string",
                "email" => "string",
                "date" => "DateTime",
                "time" => "DateTime",
                "timespan" => "TimeSpan",
                "datetime" => "DateTimeOffset",
                "bool" => "bool",
                "boolean" => "bool",
                "object" => "object",
                "int" => "int",
                "uint" => "uint",
                "tinyint" => "int",
                "bigint" => "long",
                "money" => "decimal",
                "smallmoney" => "decimal",
                "decimal" => "decimal",
                "real" => "single",
                "float" => "single",
                "bigreal" => "double",
                "bigfloat" => "double",
                _ => "string"
            };

        }

    }
}