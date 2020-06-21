using System;
using Shura.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Domain.Entities {
    public class GeoFileActivityRecord : Entity<long>, IHasIsDeleted {
        public virtual GeoFile GeoFile { get; set; }
        public virtual User User { get; set; }
        public virtual ActivityType ActivityType { get; set; }
        public virtual bool IsDeleted { get; set; }
        public virtual DateTime Occurred { get; set; }
    }
}