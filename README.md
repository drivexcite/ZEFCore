# Less database roundtrips with Z Entity Frameowrk Plus Core.

This repository contains a sample application that demostrates two strategies for reducing the number of database roundtrips in an ASP.NET Core Application using an Entity Framework Core data access layer.

## First strategy: Bundling queries.
In this first strategy, all the requests made to the database for validation purposes, are deferred until any value of the first query of the bundle is needed, then Z EF Plus will behind the scenes bundle the queries in a single roundtrip, therefore reducing the latency of the entire data access interaction.

The keys to this strategy are the `DeferredFirstOrDefault` and the `FutureValue` methods of ZEF.

```csharp
var typeFuture = _db.Types.DeferredFirstOrDefault(t => t.TypeId == applicationDto.TypeId).FutureValue();
var environmentFuture = _db.Environments.DeferredFirstOrDefault(e => e.EnvironmentId == applicationDto.EnvironmentId).FutureValue();
var ownerFuture = _db.Owners.DeferredFirstOrDefault(o => o.OwnerId == applicationDto.OwnerId).FutureValue();
var versionFuture = _db.Versions.DeferredFirstOrDefault(v => v.VersionId == applicationDto.VersionId).FutureValue();
var stageFuture = _db.Stages.DeferredFirstOrDefault(s => s.StageId == applicationDto.StageId).FutureValue();
var existingApplicationFuture = _db.Applications.DeferredFirstOrDefault(a => a.Name == applicationDto.Name).FutureValue();

var type = await typeFuture.ValueAsync();
var environment = await environmentFuture.ValueAsync();
var owner = await ownerFuture.ValueAsync();
var version = await versionFuture.ValueAsync();
var stage = await stageFuture.ValueAsync();
var existingApplication = await existingApplicationFuture.ValueAsync();

if (type == null)
	return BadRequest($"Invalid TypeId = {applicationDto.TypeId}");

if (environment == null)
	return BadRequest($"Invalid EnvironmentId = {applicationDto.EnvironmentId}");

if (owner == null)
	return BadRequest($"Invalid OwnerId = {applicationDto.OwnerId}");

if (version == null)
	return BadRequest($"Invalid VersionId = {applicationDto.VersionId}");

if (stage == null)
	return BadRequest($"Invalid StageId = {applicationDto.StageId}");

if (existingApplication != null)
	return BadRequest($"Application with 'Name = {applicationDto.Name}' already exists");

var application = new Application
{
	Name = applicationDto.Name,
	Type = type,
	Environment = environment,
	Owner = owner,
	Version = version,
	Stage = stage
};

_db.Applications.Add(application);
await _db.SaveChangesAsync();

return new ObjectResult(application.ToApplicationDto()) { StatusCode = 201 };

```

## Second strategy: Using a second-level cache.
This strategy consists in caching results that rarely ever change, and caching them for subsequent queries. This strategy, unlike the bundling, can lead to stale data, so the cache expiration remains the most important parameter while leveraging this strategy.

The key to this second strategy are the `DeferredFirstOrDefault` and the `FromCacheAsync` methods of ZEF. Notice that the second method takes parameters to configure the caching expiration policy.

```csharp
var type = await _db.Types.DeferredFirstOrDefault(t => t.TypeId == applicationDto.TypeId).FromCacheAsync();

if (type == null)
	return BadRequest($"Invalid TypeId = {applicationDto.TypeId}");

var environment = await _db.Environments.DeferredFirstOrDefault(e => e.EnvironmentId == applicationDto.EnvironmentId).FromCacheAsync();

if (environment == null)
	return BadRequest($"Invalid EnvironmentId = {applicationDto.EnvironmentId}");

var owner = await _db.Owners.DeferredFirstOrDefault(o => o.OwnerId == applicationDto.OwnerId).FromCacheAsync();

if (owner == null)
	return BadRequest($"Invalid OwnerId = {applicationDto.OwnerId}");

var version = await _db.Versions.DeferredFirstOrDefault(v => v.VersionId == applicationDto.VersionId).FromCacheAsync();

if (version == null)
	return BadRequest($"Invalid VersionId = {applicationDto.VersionId}");

var stage = await _db.Stages.DeferredFirstOrDefault(s => s.StageId == applicationDto.StageId).FromCacheAsync();

if (stage == null)
	return BadRequest($"Invalid StageId = {applicationDto.StageId}");

var existingApplication = await _db.Applications.DeferredFirstOrDefault(a => a.Name == applicationDto.Name).FromCacheAsync();

if (existingApplication != null)
	return BadRequest($"Application with 'Name = {applicationDto.Name}' already exists");

var application = new Application
{
	Name = applicationDto.Name,
	Type = type,
	Environment = environment,
	Owner = owner,
	Version = version,
	Stage = stage
};

_db.Applications.Add(application);
await _db.SaveChangesAsync();

return new ObjectResult(application.ToApplicationDto()) { StatusCode = 201 };
```
