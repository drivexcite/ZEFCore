using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ClientApi.Dtos;
using ClientApi.Entities;
using ClientApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var type = _db.Types.FirstOrDefault(t => t.TypeId == applicationDto.TypeId);

            if (type == null)
                return BadRequest($"Invalid TypeId = {applicationDto.TypeId}");

            var environment = _db.Environments.FirstOrDefault(e => e.EnvironmentId == applicationDto.EnvironmentId);

            if (environment == null)
                return BadRequest($"Invalid EnvironmentId = {applicationDto.EnvironmentId}");

            var owner = _db.Owners.FirstOrDefault(o => o.OwnerId == applicationDto.OwnerId);

            if (owner == null)
                return BadRequest($"Invalid OwnerId = {applicationDto.OwnerId}");

            var version = _db.Versions.FirstOrDefault(v => v.VersionId == applicationDto.VersionId);

            if (version == null)
                return BadRequest($"Invalid VersionId = {applicationDto.VersionId}");

            var stage = _db.Stages.FirstOrDefault(s => s.StageId == applicationDto.StageId);

            if (stage == null)
                return BadRequest($"Invalid StageId = {applicationDto.StageId}");

            var existingApplication = _db.Applications.FirstOrDefault(a => a.Name == applicationDto.Name);
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
