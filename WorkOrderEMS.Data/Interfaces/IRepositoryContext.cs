using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkOrderEMS.Data.Interfaces
{
    /// <summary>IRepositoryContext
    /// Created BY:     Nagendra Upwanshi
    /// Created On:     Aug-21-2014 
    /// Created For:    IRepositoryContext 
    /// </summary>
    public interface IRepositoryContext
    {
        IObjectSet<T> GetObjectSet<T>() where T : class;
        ObjectContext ObjectContext { get; }
        int SaveChanges();
        void Terminate();
    }
}
