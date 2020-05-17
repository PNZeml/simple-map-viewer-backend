using Shura.Domain.Entities;

namespace SimpleMapViewer.Domain.Entities {
    public class GeoFileComment : Entity<long>, IHasIsDeleted {
        public virtual string Comment { get; set; }
        public virtual decimal X { get; set; }
        public virtual decimal Y { get; set; }
        public virtual GeoFile GeoFile { get; set; }
        public virtual User User { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}