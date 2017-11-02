using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using WorkOrderEMS.Data.Interfaces;
using WorkOrderEMS.Data;
using System.Data.SqlClient;
using WorkOrderEMS.Data.RepositaryContext;
namespace WorkOrderEMS.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T : class
    {
        private IRepositoryContext _repositoryContext;
        private IObjectSet<T> _objectSet;
        public BaseRepository() : this(new RepositoryContext()) { }

        public BaseRepository(IRepositoryContext repositoryContext)
        {
            _repositoryContext = repositoryContext = repositoryContext ?? new RepositoryContext();
            _objectSet = _repositoryContext.GetObjectSet<T>();
        }

        public IRepositoryContext RepositoryContext
        {
            get
            {
                return this._repositoryContext;
            }
        }

        public IObjectSet<T> ObjectSet
        {
            get
            {
                return _objectSet;
            }
        }

        public void Add(T entity)
        {        
            this.ObjectSet.AddObject(entity);
            this.RepositoryContext.SaveChanges();         
        }

        public Int32 AddWithReturn(T entity)
        {
            this.ObjectSet.AddObject(entity);
            return this.RepositoryContext.SaveChanges();
        }

        public void BulkAdd(IList<T> entitiesCollection)
        {
            foreach (var T in entitiesCollection)
            {
                this.ObjectSet.AddObject(T);
            }
            this.RepositoryContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            this.ObjectSet.DeleteObject(entity);
            this.RepositoryContext.SaveChanges();
        }

        public void DeleteAll(Expression<Func<T, bool>> whereCondition)
        {
            var entities = this.GetAll(whereCondition);
            foreach (T entity in entities)
            {
                this.ObjectSet.DeleteObject(entity);
            }
            this.RepositoryContext.SaveChanges();
        
        }

        public IList<T> GetAll()
        {
            return this.ObjectSet.ToList<T>();
        }

        public IList<T> GetAll(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).ToList<T>();
        }

        public T GetSingle(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).FirstOrDefault<T>();
        }

        public T GetSingleOrDefault(Expression<Func<T, bool>> whereCondition)
        {
            var result = this.ObjectSet.Where(whereCondition).FirstOrDefault<T>();
            if (result == null)
                return Activator.CreateInstance<T>();
            return result;
        }

        public void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public void SaveChanges()
        {
            this.RepositoryContext.SaveChanges();
        }

        public IQueryable<T> GetQueryable()
        {
            return this.ObjectSet.AsQueryable<T>();
        }

        public IQueryable<T> GetQuery()
        {
            return ObjectSet;
        }

        public long Count()
        {
            return this.ObjectSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).LongCount<T>();
        }
        public void Update(T entity)
        {
            this.ObjectSet.Attach(entity);
            this._repositoryContext.ObjectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
            SaveChanges();
        }

        public void Dispose()
        {

        }

        public void ListAdd(List<T> entitiesCollection)
        {
            foreach (var o in entitiesCollection)
            {
                this.ObjectSet.AddObject(o);
            }
        }

        public void ListUpdate(List<T> entitiesCollection)
        {
            foreach (var o in entitiesCollection)
            {
                this.ObjectSet.Attach(o);
                this._repositoryContext.ObjectContext.ObjectStateManager.ChangeObjectState(o, EntityState.Modified);
            }
        }

        public void ListDelete(List<T> entities)
        {
            foreach (T entity in entities)
            {
                this.ObjectSet.DeleteObject(entity);
            }
        }

        public void BulkUpdate(T entity)
        {
            this.ObjectSet.Attach(entity);
            this._repositoryContext.ObjectContext.ObjectStateManager.ChangeObjectState(entity, EntityState.Modified);
        }

        #region protected methods

        protected IQueryable<T> GetQuery(Expression<Func<T, bool>> whereClause)
        {
            var entities = _repositoryContext.GetObjectSet<T>();
            //var entities = _context.CreateObjectSet<T>();
            return whereClause != null ? entities.Where(whereClause) : entities;
        }

        protected IQueryable<T> GetList<K>(Expression<Func<T, bool>> whereClause, Expression<Func<T, K>> orderBy, out int totalRecords,
            SortDirection sortDirection = SortDirection.Ascending, int pageNumber = 1, int pageSize = 100)
        {
            return GetList(GetQuery(whereClause), orderBy, out totalRecords, sortDirection, pageNumber, pageSize);
        }

        protected IQueryable<T> GetList<K>(IQueryable<T> query, Expression<Func<T, K>> orderBy, out int totalRecords,
            SortDirection sortDirection = SortDirection.Ascending, int pageNumber = 1, int pageSize = 100)
        {
            return EntityHelper<T>.SortAndFilterRecords(query, orderBy, out totalRecords, sortDirection, pageNumber, pageSize);
        }

        protected IQueryable<T> GetList<K>(IQueryable<T> query, Expression<Func<T, K>> orderBy,
            SortDirection sortDirection = SortDirection.Ascending)
        {
            return EntityHelper<T>.SortAndFilterRecords(query, orderBy, sortDirection);
        }

        //Added by Gaurav Chauhan on 27th Jan 2014
        public IEnumerable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _objectSet.Where(predicate);
        }
        #endregion
       
        
    }
}
