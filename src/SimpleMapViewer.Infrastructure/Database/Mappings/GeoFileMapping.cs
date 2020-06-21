using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class GeoFileMapping : ClassMap<GeoFile> {
        public GeoFileMapping() {
            Table("GEO_FILES");
            Id(x => x.Id)
                .GeneratedBy.Native("GEO_FILES_ID_SEQ");
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Created)
                .Not.Nullable();
            Map(x => x.Modified)
                .Nullable();
            Map(x => x.Opened)
                .Nullable();
            Map(x => x.Size)
                .Not.Nullable();
            Map(x => x.IsDeleted)
                .Column("IS_DELETED")
                .Not.Nullable();
        }
    }
}