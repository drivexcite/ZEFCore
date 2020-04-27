using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClientApi.Dtos
{
    public class ApplicationDto
    {
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public string Type { get; set; }
        public int EnvironmentId { get; set; }
        public string Environment { get; set; }
        public int OwnerId { get; set; }
        public string Owner { get; set; }
        public int VersionId { get; set; }
        public string Version { get; set; }
        public int StageId { get; set; }
        public string Stage { get; set; }
    }
}
