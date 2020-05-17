using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class UserGeoFilesRelationMapping : ClassMap<UserGeoFilesRelation> {
        public UserGeoFilesRelationMapping() {
            Table("USER_GEO_FILES_RELS");
            Id(x => x.Id)
                .GeneratedBy.Native("USER_GEO_FILES_REL_ID_SEQ");
            References(x => x.User)
                .Column("USER_ID")
                .Not.Nullable();
            References(x => x.GeoFile)
                .Column("GEO_FILE_ID")
                .Not.Nullable();
            Map(x => x.AccessType)
                .Column("ACCESS_TYPE")
                .CustomType<AccessType>();
            Map(x => x.IsDeleted)
                .Column("IS_DELETED")
                .Not.Nullable();
        }
    }
}