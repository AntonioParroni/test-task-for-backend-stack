using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        DbContext _context;
        DbSet<TEntity> _dbSet;
 
        public GenericRepository(DbContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }
 
        public IEnumerable<TEntity> Get()
        {
            return _dbSet.AsNoTracking().ToList();
        }
         
        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _context.Set<TEntity>().Where(predicate).ToList();
        }
        public TEntity FindById(int id)
        {
            return _dbSet.Find(id);
        }
 
        public void Create(TEntity item)
        {
            _dbSet.Add(item);
            _context.SaveChanges();
        }
        public void Update(TEntity item)
        {
            _context.Entry(item).State = EntityState.Modified;
            _context.SaveChanges();
        }
        public void Remove(TEntity item)
        {
            _dbSet.Remove(item);
            _context.SaveChanges();
        }
    }
}