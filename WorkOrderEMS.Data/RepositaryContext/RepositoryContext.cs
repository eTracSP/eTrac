using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WorkOrderEMS.Data.Interfaces;

namespace WorkOrderEMS.Data.RepositaryContext
{
    /// <summary>RepositoryContext
    /// Created BY:     Nagendra Upwanshi
    /// Created On:     Aug-21-2014 
    /// Created For:    RepositoryContext 
    /// </summary>
    public class RepositoryContext : IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "workorderEMSEntities";


        public IObjectSet<T> GetObjectSet<T>()
            where T : class
        {
            return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY).CreateObjectSet<T>();
        }

        /// <summary>
        /// Returns the active object context
        /// </summary>
        public ObjectContext ObjectContext
        {
            get
            {
                return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY);
            }
        }

        public int SaveChanges()
        {
            return this.ObjectContext.SaveChanges();
        }

        public void Terminate()
        {
            ContextManager.SetRepositoryContext(null, OBJECT_CONTEXT_KEY);
        }
    }
}
