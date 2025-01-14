using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Nox.Core.Configuration;
using Nox.Core.Exceptions;
using Nox.Core.Validation.Configuration;
using Nox.TestFixtures;
using NUnit.Framework;

namespace Nox.Core.Tests;

public class ConfigurationTests: ConfigurationTestFixture
{
    [Test]
    public void Can_Load_Nox_Configuration_From_Yaml_Definitions()
    {
        var appSettings = ConfigurationHelper.GetNoxAppSettings();
        var configurator = new NoxConfigurator(appSettings!["Nox:DefinitionRootPath"]!);
        var config = configurator.LoadConfiguration();
        Assert.That(config, Is.Not.Null);
        Assert.That(config!.Name, Is.EqualTo("Test"));
        Assert.That(config.Description, Is.EqualTo("Test Microservice"));
        Assert.That(config.DefinitionFileName, Is.Not.Empty);
        Assert.That(config.EndpointProvider, Is.EqualTo(""));
        //Api
        Assert.That(config.Apis, Is.Not.Null);
        Assert.That(config.Apis!.Count, Is.EqualTo(1));                
        Assert.That(config.Apis![0].Name, Is.EqualTo("TestApi"));
        Assert.That(config.Apis![0].Description, Is.EqualTo("Test entity controller and endpoints"));
        Assert.That(config.Apis![0].DefinitionFileName, Is.Not.Empty);
        Assert.That(config.Apis![0].Routes!.Count, Is.EqualTo(3));
        Assert.That(config.Apis![0].Routes![0].Name, Is.EqualTo("Persons"));
        Assert.That(config.Apis![0].Routes![0].Description, Is.EqualTo("Returns all persons"));
        Assert.That(config.Apis![0].Routes![0].HttpVerb, Is.EqualTo("GET"));
        Assert.That(config.Apis![0].Routes![0].TargetUrl, Is.EqualTo("odata/Persons"));
        Assert.That(config.Apis![0].Routes![0].Responses!.Count, Is.EqualTo(1));
        //Database
        Assert.That(config.Database, Is.Not.Null);
        Assert.That(config.Database!.Name, Is.EqualTo("Test"));
        Assert.That(config.Database!.ConnectionString, Is.EqualTo("Data Source=./DataTest.db"));
        //Entities
        Assert.That(config.Entities, Is.Not.Null);
        Assert.That(config.Entities!.Count, Is.EqualTo(2));
        //Person
        var vehicle = config.Entities.FirstOrDefault(e => e.Name == "Vehicle");
        Assert.That(vehicle, Is.Not.Null);
        Assert.That(vehicle!.Attributes.Count, Is.EqualTo(3));
        Assert.That(vehicle.DefinitionFileName, Is.Not.Empty);
        Assert.That(vehicle.Description, Is.EqualTo("Vehicles"));
        Assert.That(vehicle.Name, Is.EqualTo("Vehicle"));
        Assert.That(vehicle.PluralName, Is.EqualTo("Vehicles"));
        Assert.That(vehicle.RelatedParents!.Count, Is.EqualTo(1));
        Assert.That(vehicle.Schema, Is.EqualTo("dbo"));
        Assert.That(vehicle.Table, Is.EqualTo("Vehicle"));

        //Loaders        
        Assert.That(config.Loaders, Is.Not.Null);
        Assert.That(config.Loaders!.Count, Is.EqualTo(2));
        var vehicleLoader = config.Loaders.FirstOrDefault(l => l.Name == "VehicleLoader");
        Assert.That(vehicleLoader, Is.Not.Null);
        Assert.That(vehicleLoader!.DefinitionFileName, Is.Not.Empty);
        Assert.That(vehicleLoader.Description, Is.EqualTo("Loads vehicle data"));
        Assert.That(vehicleLoader.LoadStrategy, Is.Not.Null);
        Assert.That(vehicleLoader.LoadStrategy!.Columns, Is.Not.Null);
        Assert.That(vehicleLoader.LoadStrategy!.Columns[0], Is.EqualTo("CreateDate"));
        Assert.That(vehicleLoader.Messaging, Is.Not.Empty);
        Assert.That(vehicleLoader.Messaging!.Count, Is.EqualTo(1));
        Assert.That(vehicleLoader.Name, Is.EqualTo("VehicleLoader"));
        Assert.That(vehicleLoader.Schedule, Is.Not.Null);
        Assert.That(vehicleLoader.Schedule!.Start, Is.EqualTo("Daily at 2am UTC"));
        Assert.That(vehicleLoader.Sources, Is.Not.Empty);
        Assert.That(vehicleLoader.Sources!.Count, Is.EqualTo(1));
        Assert.That(vehicleLoader.Sources![0].DataSource, Is.EqualTo("TestDataSource2"));
        Assert.That(vehicleLoader.Sources![0].MinimumExpectedRecords, Is.EqualTo(30));
        Assert.That(vehicleLoader.Sources![0].Query, Is.Not.Empty);
        
        //DataSources
        Assert.That(config.DataSources, Is.Not.Null);
        Assert.That(config.DataSources!.Count, Is.EqualTo(3));
        var testDataSource = config.DataSources.FirstOrDefault(ds => ds.Name == "TestDataSource1");
        Assert.That(testDataSource, Is.Not.Null);
        Assert.That(testDataSource!.Name, Is.EqualTo("TestDataSource1"));
        Assert.That(testDataSource.Password, Is.EqualTo("password"));
        Assert.That(testDataSource.Provider, Is.EqualTo("SqLite"));
        Assert.That(testDataSource.ConnectionString, Is.EqualTo("Data Source=Test;Mode=Memory;Cache=Shared"));
        
        //Messaging Providers
        Assert.That(config.MessagingProviders, Is.Not.Null);
        Assert.That(config.MessagingProviders!.Count, Is.EqualTo(2));
        var msgProvider = config.MessagingProviders.FirstOrDefault(mp => mp.Name == "TestMessagingProvider1");
        Assert.That(msgProvider, Is.Not.Null);
        Assert.That(msgProvider!.Name, Is.EqualTo("TestMessagingProvider1"));
        Assert.That(msgProvider.Provider, Is.EqualTo("InMemory"));
    }
    
}