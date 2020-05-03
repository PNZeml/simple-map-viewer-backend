using System;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using Shura.Data;
using SimpleMapViewer.Infrastructure.Settings;

namespace SimpleMapViewer.Infrastructure.Database {
    public class Database : IDatabase<ISession>, IDisposable {
        private readonly ISessionFactory sessionFactory;

        public Database(IDatabaseSettings databaseSettings) {
            var persistenceConfig = PostgreSQLConfiguration.Standard
                .ConnectionString(x => x.Is(databaseSettings.ConnectionString));
            sessionFactory = Fluently.Configure()
                .Database(persistenceConfig)
                .Mappings(mappingConfig =>
                    mappingConfig.FluentMappings
                        .AddFromAssembly(Assembly.GetExecutingAssembly())
                        .Conventions.Add(DefaultCascade.None(), DefaultLazy.Always())
                )
                .BuildSessionFactory();
        }

        public ISession CreateSource() => sessionFactory.OpenSession();

        public void Dispose() => sessionFactory.Dispose();
    }
}