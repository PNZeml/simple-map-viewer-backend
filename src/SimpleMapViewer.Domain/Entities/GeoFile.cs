using System;

namespace SimpleMapViewer.Domain.Entities {
    public class GeoFile {
        public virtual long Id { get; set; }
        public virtual string Name { get; set; }
        public virtual DateTime UploadDate { get; set; }
        public virtual long Size { get; set; }
        public virtual User OwnerUser { get; set; } 
    }
}