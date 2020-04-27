using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClientApi.Dtos;
using ClientApi.Entities;

namespace ClientApi.Extensions
{
    public static class EntityExtensions
    {
        public static ApplicationDto ToApplicationDto(this Application application)
        {
            return new ApplicationDto
            {
                ApplicationId = application.ApplicationId,
                Name = application.Name,
                TypeId = application.Type.TypeId,
                Type = application.Type.Name,
                EnvironmentId = application.Environment.EnvironmentId,
                Environment = application.Environment.Name,
                OwnerId = application.Owner.OwnerId,
                Owner = application.Owner.Name,
                VersionId = application.Version.VersionId,
                Version = application.Version.Name,
                StageId = application.Stage.StageId,
                Stage = application.Stage.Name
            };
        }
    }
}
