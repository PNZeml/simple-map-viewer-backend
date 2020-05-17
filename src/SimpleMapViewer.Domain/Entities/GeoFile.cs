using System;
using Shura.Domain.Entities;

namespace SimpleMapViewer.Domain.Entities {
    public class GeoFile : Entity<long>, IHasIsDeleted {
        public virtual string Name { get; set; }
        public virtual DateTime Created { get; set; }
        public virtual DateTime? Modified { get; set; }
        public virtual DateTime? Opened { get; set; }
        /// <summary>
        /// File size in bytes
        /// </summary>
        public virtual long Size { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}