using FluentNHibernate.Mapping;
using SimpleMapViewer.Domain.Entities;

namespace SimpleMapViewer.Infrastructure.Database.Mappings {
    internal class UserMapping : ClassMap<User> {
        public UserMapping() {
            Table("users");
            Id(x => x.Id).GeneratedBy.Increment();
            Map(x => x.Name)
                .Not.Nullable();
            Map(x => x.Email)
                .Not.Nullable();
            Map(x => x.PasswordHashed)
                .Column("password_hashed")
                .Not.Nullable();
            Map(x => x.AccessToken)
                .Column("access_token")
                .Nullable();
            HasMany(x => x.OwnedGeoFiles)
                .Inverse()
                .LazyLoad();
        }
    }
}