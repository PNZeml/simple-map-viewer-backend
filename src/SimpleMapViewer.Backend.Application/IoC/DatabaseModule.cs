using Autofac;
using NHibernate;
using Shura.Data;
using Shura.Data.NHibernate;
using SimpleMapViewer.Infrastructure.Constants;
using SimpleMapViewer.Infrastructure.Database;

namespace SimpleMapViewer.Backend.Application.IoC {
    internal class DatabaseModule : Module {
        protected override void Load(ContainerBuilder builder) {
            builder
                .RegisterType<Database>()
                .As<IDatabase<ISession>>()
                .SingleInstance();
            builder
                .RegisterType<NHibernateUnitOfWork>()
                .As<IUnitOfWork<ISession>>()
                .InstancePerMatchingLifetimeScope(LifetimeScopeTags.UNIT_OF_WORK);
        }
    }
}