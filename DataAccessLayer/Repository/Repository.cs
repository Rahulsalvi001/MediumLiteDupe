using DataAccessLayer.Data;
using DataAccessLayer.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAccessLayer.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly AppDBContext _db;
        internal DbSet<T> dbSet;
        public Repository(AppDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(int id, string includProperties = null)
        {
            return dbSet.Find(id);
        }

        public IEnumerable<T> GetAll(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, Func<System.Linq.IQueryable<T>, System.Linq.IOrderedQueryable<T>> orderBy = null, string includProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includProperties != null)
            {
                foreach (var includeProp in includProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            return query.ToList();
        }

        public T GetFirstAndDefault(System.Linq.Expressions.Expression<Func<T, bool>> filter = null, string includProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includProperties != null)
            {
                foreach (var includeProp in includProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }

            return query.FirstOrDefault();
        }

        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            dbSet.Remove(entity);
        }

        public void Remove(T entity)
        {
            dbSet.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entity)
        {
            dbSet.RemoveRange(entity);
        }
    }
}
