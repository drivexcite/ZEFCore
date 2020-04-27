using System.Collections.Generic;
using System.Linq;
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
        private readonly FooDb _db;

        public ApplicationController(FooDb db)
        {
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
            var type = await _db.Types.FirstOrDefaultAsync(t => t.TypeId == applicationDto.TypeId);

            if (type == null)
                return BadRequest($"Invalid TypeId = {applicationDto.TypeId}");

            var environment = await _db.Environments.FirstOrDefaultAsync(e => e.EnvironmentId == applicationDto.EnvironmentId);

            if (environment == null)
                return BadRequest($"Invalid EnvironmentId = {applicationDto.EnvironmentId}");

            var owner = await _db.Owners.FirstOrDefaultAsync(o => o.OwnerId == applicationDto.OwnerId);

            if (owner == null)
                return BadRequest($"Invalid OwnerId = {applicationDto.OwnerId}");

            var version = await _db.Versions.FirstOrDefaultAsync(v => v.VersionId == applicationDto.VersionId);

            if (version == null)
                return BadRequest($"Invalid VersionId = {applicationDto.VersionId}");

            var stage = await _db.Stages.FirstOrDefaultAsync(s => s.StageId == applicationDto.StageId);

            if (stage == null)
                return BadRequest($"Invalid StageId = {applicationDto.StageId}");

            var existingApplication = await _db.Applications.FirstOrDefaultAsync(a => a.Name == applicationDto.Name);

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
