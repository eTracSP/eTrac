using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Data.Interfaces
{
    /// <summary>IRepository
    /// Created BY:     Nagendra Upwanshi
    /// Created On:     Aug-21-2014 
    /// Created For:    IRepository 
    /// </summary>
    public interface IRepository<T>
    {
        T GetSingle(Expression<Func<T, bool>> whereCondition);
        void Add(T entity);
        void Delete(T entity);
        void Attach(T entity);
        IList<T> GetAll(Expression<Func<T, bool>> whereCondition);
        IList<T> GetAll();
        IQueryable<T> GetQueryable();
        long Count(Expression<Func<T, bool>> whereCondition);
        long Count();
    }
}
