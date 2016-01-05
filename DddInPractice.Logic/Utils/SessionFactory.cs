using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;
using NHibernate.Event;

namespace DddInPractice.Logic.Utils
{
    public static class SessionFactory
    {
        private static ISessionFactory _factory;

        public static ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        public static void Init(string connectionString)
        {
            _factory = BuildSessionFactory(connectionString);
        }

        private static ISessionFactory BuildSessionFactory(string connectionString)
        {
            FluentConfiguration configuration = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2012.ConnectionString(connectionString))
                .Mappings(m => m.FluentMappings
                    .AddFromAssembly(Assembly.GetExecutingAssembly())
                    .Conventions.Add(
                        ForeignKey.EndsWith("ID"),
                        ConventionBuilder.Property
                            .When(criteria => criteria.Expect(x => x.Nullable, Is.Not.Set), x => x.Not.Nullable()))
                    .Conventions.Add<TableNameConvention>()
                    .Conventions.Add<HiLoConvention>()
                )
                .ExposeConfiguration(x =>
                {
                    x.EventListeners.PostCommitUpdateEventListeners = 
                        new IPostUpdateEventListener[] { new EventListener() };
                    x.EventListeners.PostCommitInsertEventListeners = 
                        new IPostInsertEventListener[] { new EventListener() };
                    x.EventListeners.PostCommitDeleteEventListeners =
                        new IPostDeleteEventListener[] { new EventListener() };
                    x.EventListeners.PostCollectionUpdateEventListeners = 
                        new IPostCollectionUpdateEventListener[] { new EventListener() };
                });

            return configuration.BuildSessionFactory();
        }

        public class TableNameConvention : IClassConvention
        {
            public void Apply(IClassInstance instance)
            {
                instance.Table("[dbo].[" + instance.EntityType.Name + "]");
            }
        }

        public class HiLoConvention : IIdConvention
        {
            public void Apply(IIdentityInstance instance)
            {
                instance.Column(instance.EntityType.Name + "ID");
                instance.GeneratedBy.HiLo("[dbo].[Ids]", "NextHigh", "9", "EntityName = '" + instance.EntityType.Name + "'");
            }
        }
    }
}
