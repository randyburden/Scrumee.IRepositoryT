using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace Scrumee.Repositories.Interfaces
{
    public interface IRepository<T> : IQueryable<T>, ICollection<T> where T : class
    {
        ISession GetSession();

        T Get( object id );
    }
}
