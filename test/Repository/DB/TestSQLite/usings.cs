global using System;
global using System.Configuration;

global using Microsoft.EntityFrameworkCore;
global using Microsoft.EntityFrameworkCore.ChangeTracking;
global using Microsoft.EntityFrameworkCore.Metadata.Builders;
global using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
global using Microsoft.EntityFrameworkCore.ValueGeneration;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Logging;

global using vm2.Repository.Abstractions;
global using vm2.Repository.DB.SQLite;
global using vm2.Repository.DB.TestSQLite.Mapping;
global using vm2.Repository.DB.TestSQLite.Mapping.Converters;
global using vm2.Repository.DB.TestSQLite.Mapping.Dimensions;
global using vm2.Repository.DB.TestSQLite.Mapping.Generators;
global using vm2.Repository.EntityFramework.Models.Mapping;
global using vm2.Repository.TestDomain;
global using vm2.Repository.TestDomain.Dimensions;
global using vm2.UlidType;
