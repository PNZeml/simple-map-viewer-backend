using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class GeoFileCommentMapping : ClassMap<GeoFileComment> {
        public GeoFileCommentMapping() {
            Table("GEO_FILE_COMMENTS");
            Id(x => x.Id)
                .GeneratedBy.Native("GEO_FILE_COMMENTS_ID_SEQ");
            Map(x => x.Comment)
                .Not.Nullable();
            Map(x => x.X)
                .Not.Nullable();
            Map(x => x.Y)
                .Not.Nullable();
            References(x => x.GeoFile)
                .Column("GEO_FILE_ID")
                .Not.Nullable();
            References(x => x.User)
                .Column("USER_ID")
                .Not.Nullable();
            Map(x => x.IsDeleted)
                .Column("IS_DELETED")
                .Not.Nullable();
        }
    }
}