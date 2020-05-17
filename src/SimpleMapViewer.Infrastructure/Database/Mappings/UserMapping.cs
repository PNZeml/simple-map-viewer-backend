using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class UserMapping : ClassMap<User> {
        public UserMapping() {
            Table("USERS");
            Id(x => x.Id)
                .GeneratedBy.Native("USER_ID_SEQ");
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Email)
                .Not.Nullable();
            Map(x => x.PasswordHashed)
                .Column("PASSWORD_HASHED")
                .Not.Nullable();
            Map(x => x.Avatar)
                .Column("AVATAR")
                .Not.Nullable();
            Map(x => x.AccessToken)
                .Column("ACCESS_TOKEN")
                .Nullable();
            Map(x => x.IsDeleted)
                .Column("IS_DELETED")
                .Not.Nullable();
        }
    }
}