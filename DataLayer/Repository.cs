using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer;
using System.Data.Entity;
using DataLayer.KisanModel;
using System.Linq.Expressions;
using System.Data;

namespace DataLayer
{
    public class Repository<TEntity> : Irepository<TEntity> where TEntity : class
    {
        private DbContext Context;
        private DbSet<TEntity> DBSet;
        public Repository(KisanWeighningEntities context)
        {
            Context = context as DbContext;
            DBSet = Context.Set<TEntity>();
        }

        public TEntity Get(object id)
        {
            TEntity model = DBSet.Find(id);
            return model;
        }      
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return DBSet.Where(predicate).AsEnumerable();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DBSet.AsEnumerable<TEntity>();
        }

        public TEntity Add(TEntity model, bool persist = false)
        {
            this.DBSet.Add(model);
            this.Context.SaveChanges();
            return model;
        }

        public TEntity Update(TEntity model)
        {
             this.DBSet.Attach(model);
            Context.Entry(model).State = EntityState.Modified;
            this.Context.SaveChanges();
            return model;
        }

        public void Remove(object id, bool persist = false)
        {
            TEntity model = DBSet.Find(id);
            Remove(model, persist);
        }

        public void Remove(TEntity model, bool persist = false)
        {
            if (model != null)
            {
                Context.Entry<TEntity>(model).State = EntityState.Deleted;
                Save(persist);
            }
        }

        public void Save()
        {
            Save(true);
        }

        private void Save(bool persist)
        {
            if (persist)
            {
                Context.SaveChanges();
            }
        }
    }
}
