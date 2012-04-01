using Scrumee.Repositories;
using Scrumee.Repositories.Interfaces;
using StructureMap;
using NHibernate;

namespace Scrumee.Infrastructure
{
    public class StructureMapBootstrapper
    {
        /// <summary>
        /// Initializes Structuremap
        /// </summary>
        public static void BootstrapStructureMap()
        {
            ObjectFactory.Initialize( x =>
            {
                // Registering the ISessionFactory as a singleton
                x.For<ISessionFactory>()
                    .Singleton()
                    .Use( NHibernateBootstrapper.SessionFactory );

                // Caching the ISession in the Http context
                // Example of how to manually get the ISession: var session = ObjectFactory.GetInstance<ISession>();
                x.For<ISession>()
                    .HttpContextScoped()
                    .Use( context => context.GetInstance<ISessionFactory>().OpenSession() );

                // Registering the IRepository<T>
                x.For( typeof ( IRepository<> ) ).Use( typeof(Repository<>) );
            } );
        }

        /// <summary>
        /// Disposes the ISession at the end of each web request
        /// </summary>
        public static void ReleaseAndDisposeAllHttpScopedObjects()
        {
            ObjectFactory.ReleaseAndDisposeAllHttpScopedObjects();
        }
    }
}
