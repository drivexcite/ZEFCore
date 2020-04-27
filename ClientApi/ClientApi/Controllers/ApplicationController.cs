using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApi.Dtos;
using ClientApi.Entities;
using ClientApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace ClientApi.Controllers
{
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactoryFactory;
        private readonly FooDb _db;

        public ApplicationController(IHttpClientFactory httpClientFactory, FooDb db)
        {
            _httpClientFactoryFactory = httpClientFactory;
            _db = db;
        }

        [HttpGet]
        [Route("applications")]
        public IEnumerable<ApplicationDto> Get()
        {
            return (
                from a in _db.Applications
                    .Include(a => a.Type)
                    .Include(a => a.Environment)
                    .Include(a => a.Owner)
                    .Include(a => a.Version)
                    .Include(a => a.Stage)
                select a.ToApplicationDto()
            ).ToList();
        }

        [HttpPost]
        [Route("applications")]
        public async Task<IActionResult> Create(ApplicationDto applicationDto)
        {
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
        }
    }
}
