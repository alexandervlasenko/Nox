using Microsoft.Extensions.Logging;
using Nox.Core.Constants;
using Serilog;
using YamlDotNet.Serialization;

namespace Nox.Core.Configuration;

public class NoxConfigurator
{
    private readonly string _designRoot;
    private readonly IDeserializer _deserializer;
    private NoxConfiguration? _config;
    
    public NoxConfigurator(string designRoot)
    {
        _designRoot = designRoot;
        _deserializer = new DeserializerBuilder()
            .Build();
    }

    public NoxConfiguration LoadConfiguration()
    {
        _config = ReadServiceDefinition();
        _config.Entities = ReadEntityDefinitionsFromFolder();
        _config.Loaders = ReadLoaderDefinitionsFromFolder();
        _config.Apis = ReadApiDefinitionsFromFolder();
        return _config;
    }

    private NoxConfiguration ReadServiceDefinition()
    {
        return Directory
            .EnumerateFiles(_designRoot, FileExtension.ServiceDefinition, SearchOption.AllDirectories)
            .Select(f =>
            {
                var config = _deserializer.Deserialize<NoxConfiguration>(ReadDefinitionFile(f));
                config.DefinitionFileName = Path.GetFullPath(f);
                config.Database!.DefinitionFileName = Path.GetFullPath(f);
                return config;
            })
            .First();
    }
    
    private List<EntityConfiguration> ReadEntityDefinitionsFromFolder()
    {
        return Directory
            .EnumerateFiles(_designRoot, FileExtension.EntityDefinition, SearchOption.AllDirectories)
            .Select(f =>
            {
                var entity = _deserializer.Deserialize<EntityConfiguration>(ReadDefinitionFile(f));
                entity.DefinitionFileName = Path.GetFullPath(f);
                entity.Attributes.ToList().ForEach(a => { a.DefinitionFileName = Path.GetFullPath(f); });
                return entity;
            })
            .ToList();
    }
        
    private List<LoaderConfiguration> ReadLoaderDefinitionsFromFolder()
    {
        var loaders = Directory
            .EnumerateFiles(_designRoot, FileExtension.LoaderDefinition, SearchOption.AllDirectories)
            .Select(f =>
            {
                var loader = _deserializer.Deserialize<LoaderConfiguration>(ReadDefinitionFile(f));
                loader.DefinitionFileName = Path.GetFullPath(f);
                loader.Sources!.ToList().ForEach(s => { s.DefinitionFileName = Path.GetFullPath(f); });
                return loader;
            });
        return loaders.ToList();
    }
        
    private List<ApiConfiguration> ReadApiDefinitionsFromFolder()
    {
        return Directory
            .EnumerateFiles(_designRoot, FileExtension.ApiDefinition, SearchOption.AllDirectories)
            .Select(f =>
            {
                var api = _deserializer.Deserialize<ApiConfiguration>(ReadDefinitionFile(f));
                api.DefinitionFileName = Path.GetFullPath(f);
                return api;
            })
            .ToList();
    }
    
    private string ReadDefinitionFile(string fileName)
    {
        Log.Information("Reading definition from {fileName}", fileName.Replace('\\', '/'));
        return File.ReadAllText(fileName);
    }
}