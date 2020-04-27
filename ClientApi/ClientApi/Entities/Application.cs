using System;

namespace ClientApi.Entities
{
    public partial class Application
    {
        public int ApplicationId { get; set; }
        public string Name { get; set; }
        public int TypeId { get; set; }
        public int EnvironmentId { get; set; }
        public int OwnerId { get; set; }
        public int VersionId { get; set; }
        public int StageId { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual Environment Environment { get; set; }
        public virtual Owner Owner { get; set; }
        public virtual Stage Stage { get; set; }
        public virtual Type Type { get; set; }
        public virtual Version Version { get; set; }
    }
}
