using Shura.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Domain.Entities {
    public class UserGeoFileRelation : Entity<long>, IHasIsDeleted {
        public virtual User User { get; set; }
        public virtual GeoFile GeoFile { get; set;}
        public virtual AccessType AccessType { get; set; }
        public virtual bool IsDeleted { get; set; }
    }
}