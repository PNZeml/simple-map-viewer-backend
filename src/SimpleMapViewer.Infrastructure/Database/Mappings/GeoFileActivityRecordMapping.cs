using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;
using SimpleMapViewer.Domain.Enums;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class GeoFileActivityRecordMapping : ClassMap<GeoFileActivityRecord> {
        public GeoFileActivityRecordMapping() {
            Table("GEO_FILE_ACTIVITY_RECORDS");
            Id(x => x.Id)
                .GeneratedBy.Native("GEO_FILE_ACTIVITY_RECORDS_ID_SEQ");
            References(x => x.GeoFile)
                .Column("GEO_FILE_ID")
                .Not.Nullable();
            References(x => x.User)
                .Column("USER_ID")
                .Not.Nullable();
            Map(x => x.ActivityType)
                .Column("ACTIVITY_TYPE")
                .CustomType<AccessType>()
                .Not.Nullable();
            Map(x => x.Occurred)
                .Not.Nullable();
            Map(x => x.IsDeleted)
                .Column("IS_DELETED")
                .Not.Nullable();
        }
    }
}