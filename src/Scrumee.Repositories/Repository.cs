using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Linq;
using Scrumee.Repositories.Interfaces;

namespace Scrumee.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ISession _session;

        #region Constructor

        public Repository( ISession session )
        {
            _session = session;
        }

        #endregion Constructor

        #region Implementation of IRepository<T>

        /// <summary>
        /// Get the underlying NHibernate Session for this repository
        /// </summary>
        /// <returns>This repositories NHibernate session</returns>
        public ISession GetSession()
        {
            return _session;
        }

        /// <summary>
        /// Gets a single entity
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>A single entity</returns>
        public T Get( object id )
        {
            return _session.Get<T>( id );
        }

        #endregion Implementation of IRepository<T>

        #region Implementation of IEnumerable

        public IEnumerator<T> GetEnumerator()
        {
            return All().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion Implementation of IEnumerable

        #region Implementation of IQueryable

        public Expression Expression
        {
            get { return All().Expression; }
        }

        public Type ElementType
        {
            get { return All().ElementType; }
        }

        public IQueryProvider Provider
        {
            get { return All().Provider; }
        }

        #endregion Implementation of IQueryable

        #region Implementation of ICollection<T>

        public void Add( T item )
        {
            using ( var tx = _session.BeginTransaction() )
            {
                _session.SaveOrUpdate( item );

                tx.Commit();
            }
        }

        public void Clear()
        {
        }

        public bool Contains( T item )
        {
            return All().Any( x => x == item );
        }

        public void CopyTo( T[] array, int arrayIndex )
        {
            All().Skip( arrayIndex ).ToArray().CopyTo( array, arrayIndex );
        }

        public bool Remove( T item )
        {
            using ( var tx = _session.BeginTransaction() )
            {
                _session.Delete( item );

                tx.Commit();
            }

            return true;
        }

        public int Count
        {
            get { return All().Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        #endregion

        private IQueryable<T> All()
        {
            return _session.Query<T>();
        }
    }
}
