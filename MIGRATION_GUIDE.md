# WORK IN PROGRESS

This is still just a collection of notes. Will slowly evolve as the API for
1.0 becomes clear.

# Migration guide 0.x to 1.x

EventFlow 1.x introduces carefully considered breaking API changes. Traditionally
EventFlow has a strict policy regarding stable APIs, with the introduction of
the 1.0 release, it is the first time any breaking change has been made to the
public API surface.

Here is the general motivation for introducing breaking changes to EventFlow.

- The initial version of EventFlow had its own IoC and logger implementation,
  but with the introduction of standardized `Microsoft.Extensions` packages,
  many of these custom made implementations can be removed
- Focus on LTS versions of .NET (Core) and removing support .NET Framework.
- Fix misssssspelled API
- Add obviously missing async/await on critical methods
- Remove non-async methods wrapper methods

## Notable new features in 1.x

While the main focus of 1.x is to bring EventFlow up to speed with the latest
standards, there some changes/features that has been added as well. Features
that wasn't possible to add before as introducing them would cause breaking changes.

- **Multiple MSSQL connection strings:** Its now possible to have read models
  outside the main database by adding a `[SqlReadModelConnectionStringName]`
  attribute to the read models. The named connection string is then used for
  that read model. To configure the named connection strings, provide them
  during the initial configuration.

  ```csharp
  MsSqlConfiguration.New
    .SetConnectionString(/* events connection string */)
    .SetConnectionString("my-awesome-read-model", /* alternative connection string */)
  ```

  If the connection string is not known at initialization, provide your own instance
  of the `IMsSqlConfiguration` which now has a new method.
  
  ```csharp
   Task<string> GetConnectionStringAsync(
      Label label,
      string name,
      CancellationToken cancellationToken);
  ```

  This allows for connection strings to be fetched runtime from external sources.

## Changes to supported .NET versions

With the 1.x release, EventFlow limits the amount of supported .NET versions, to
that of official [.NET (Core) LTS versions](https://dotnet.microsoft.com/en-us/platform/support/policy).
Support for non-LTS versions will be limited, but do expect to have EventFlow lag a little
behind when cutting support on older versions.

As of the 1.0 release, EventFlow supports the following framework versions.

- `netstandard2.1`
- `netcoreapp3.1`
- `net6.0`

Note that this enabled the use of `IAsyncEnumerable` which is going to be a key driver
for some of the upcoming features of EventFlow. 

## Data in event stores

Upgrading EventFlow should **never** break existing data in event stores, not even
between major versions. All data currently in event stores will work with 1.x
releases. However, it _might_ not be possible to do a rollback from 1.x to 0.x.

## Recommended strategy for migrating 0.x to 1.x

Here is a few recommendations that might be useful when planning the migration
of EventFlow from 0.x to 1.x. 

- Since there is no change to the underlying storage, creating a release that
  only has EventFlow upgraded is highly recommended. This enables easy rollback
  if you encounter unexpected problems

## NuGet packages removed

With the move toward the standardized Microsoft extensions packages and removal
of support for .NET Framework, there are a few NuGet packages that will no
longer be supported.

- `EventFlow.Autofac`
   
   Since EventFlow uses the new `Microsoft.Extensions.DependencyInjection` for
   handling IoC, its possible to install the Autofac adapter package instead,
   thus rendering the package obsolete.

- `EventFlow.DependencyInjection`

  Since the standard dependency injection is now a first class citizen in the
  core package, this package is no longer needed.

- `EventFlow.Owin`

  OWIN support has been removed as ASP.NET Core is introduced.


## Initializing EventFlow

Starting 1.0, there are a few ways you can initialize EventFlow.


### Fluent as `IServiceCollection` extension

```csharp
serviceCollection.AddEventFlow(o => 
    // Set up EventFlow here
    );
```

### Traditionally passing a `IServiceCollection` reference

```csharp
var eventFlowOptions = EventFlowOptions.New(serviceCollection)

// Set up EventFlow here
```


### Let EventFlow create the `IServiceCollection`

Useful in small tests, but should NOT be used in production setups.

```csharp
var eventFlowOptions = EventFlowOptions.New()

// Set up EventFlow here
```

Its basically a shorthand for

```csharp
var eventFlowOptions = EventFlowOptions.New(new ServiceCollection())
```


## Aligning with Microsoft extension packages

Several types have been removed from EventFlow in order to align
with the Microsoft extension packages.

- `ILog` use `ILogger` from `Microsoft.Extensions.Logger.Abstractions`
- `IResolver` use `IServiceProvider`
  from `Microsoft.Extensions.DependencyInjection.Abstractions`


## Only one interface for read models

The interfaces `IAmAsyncReadModelFor` has replaced the original `IAmReadModelFor`
leaving only async interface to implement on read models.

Originally EventFlow only had the non-async version `IAmReadModelFor`, but as it
became evident that updating read models sometimes requires the invocation of 
async method, the interface `IAmAsyncReadModelFor` was introduces as not to create
any breaking changes. Now, we remove the one and only have one interface to
implement.

## Removal of non-async method

Several non-async methods have been removed as well as the
`EventFlow.Core.AsyncHelper` which was used to implement these methods
without introducing deadlocks when running in some .NET Framework
environments.

- `IAggregateStore.Load`
- `IAggregateStore.Store`
- `IAggregateStore.Update`
- `ICommandBus.Publish`
- `IEventStore.LoadAggregate`
- `IEventStore.LoadEvents`
- `IEventStore.LoadAllEvents`
- `IQueryProcessor.Process`
- `IReadModelPopulator.Populate`
- `IReadModelPopulator.Purge`


