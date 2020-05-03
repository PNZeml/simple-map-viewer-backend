using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class GeoFileMapping : ClassMap<GeoFile> {
        public GeoFileMapping() {
            Table("geo_files");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.UploadDate)
                .Column("upload_date")
                .Not.Nullable();
            Map(x => x.Size)
                .Not.Nullable();
            References(x => x.OwnerUser)
                .Column("owner_user_id");
        }
    }
}