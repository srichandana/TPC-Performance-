using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Objects;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TPC.Context.Interfaces;

namespace TPC.Context.Infrastructure
{
    public class TPCRepositaryContext : IRepositoryContext
    {
        private const string OBJECT_CONTEXT_KEY = "PenworthyEntities";
        public IDbSet<T> GetObjectSet<T>()
           where T : class
        {
            return ContextManager.GetObjectContext(OBJECT_CONTEXT_KEY).Set<T>();
        }

        /// <summary>
        /// Returns the active object context
        /// </summary>
        public DbContext ObjectContext
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

        public void SetAutoDetectChangesEnabled(bool val)
        {
            this.ObjectContext.Configuration.AutoDetectChangesEnabled = val;
        }

        public void Terminate()
        {
            ContextManager.SetRepositoryContext(null, OBJECT_CONTEXT_KEY);
        }

    }
}
