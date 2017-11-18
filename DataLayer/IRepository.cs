using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public interface Irepository<TEntity> where TEntity : class
    {
        TEntity Add(TEntity model, bool persist = false);
        TEntity Get(object id);     
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
        IEnumerable<TEntity> GetAll();
        TEntity Update(TEntity model);
        void Remove(object id, bool persist = false);
        void Remove(TEntity model, bool persist = false);
        void Save();
    }
}
