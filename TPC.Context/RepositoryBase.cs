using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Objects;
using System.Linq.Expressions;
using TPC.Context.Interfaces;
using TPC.Context.Infrastructure;
using TPC.Context.EntityModel;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Data;

namespace TPC.Context
{
    public class RepositoryBase<T> : IRepository<T>
        where T : class
    {
        private IRepositoryContext _tpcRepoContext;
        public RepositoryBase()
            : this(new TPCRepositaryContext())
        {
        }

        public RepositoryBase(IRepositoryContext repositoryContext)
        {
            _tpcRepoContext = repositoryContext ?? new TPCRepositaryContext();
            _objectSet = repositoryContext.GetObjectSet<T>();
        }

        private IDbSet<T> _objectSet;
        public IDbSet<T> ObjectSet
        {
            get
            {
                return _objectSet;
            }
        }

        #region IRepository Members

        public void Add(T entity)
        {
            this.ObjectSet.Add(entity);
        }

        public void Delete(T entity)
        {
            this.ObjectSet.Remove(entity);
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

        public void Attach(T entity)
        {
            this.ObjectSet.Attach(entity);
        }

        public IQueryable<T> GetQueryable()
        {
            return this.ObjectSet.AsQueryable<T>();
        }

        public long Count()
        {
            return this.ObjectSet.LongCount<T>();
        }

        public long Count(Expression<Func<T, bool>> whereCondition)
        {
            return this.ObjectSet.Where(whereCondition).LongCount<T>();
        }

        public int SaveChanges()
        {
            return _tpcRepoContext.SaveChanges();
        }
        public void SetAutoDetectChangesEnabled(bool val)
        {
            _tpcRepoContext.SetAutoDetectChangesEnabled(val);
        }

        public void ExecSp(string SpName, SqlParameter[] param)
        {
            _tpcRepoContext.ObjectContext.Database.ExecuteSqlCommand("Exec " + SpName, param.ToArray());
        }

        public string ExecSpWithOutputParamter(string SpName, SqlParameter[] param)
        {
            string DivNo = _tpcRepoContext.ObjectContext.Database.SqlQuery<string>("Exec " + SpName, param.ToArray()).FirstOrDefault().ToString();
            return DivNo;
        }

        public List<KPLItem> ExecSpandReturnList(string SpName, SqlParameter[] param)
        {
            List<KPLItem> dsItem = _tpcRepoContext.ObjectContext.Database.SqlQuery<KPLItem>("Exec " + SpName, param.ToArray()).ToList();
            return dsItem;
        }
        #endregion
    }
}
