using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccessLayer.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        T Get(int id, string includProperties = null);
        IEnumerable<T> GetAll(Expression<Func<T, bool>> filter = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            string includProperties = null);
        T GetFirstAndDefault(Expression<Func<T, bool>> filter = null,string includProperties = null);
        void Add(T entity);
        void Remove(int id);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);
    }
}
