using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EfCore
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected private readonly RepositoryContext _context;
        protected private readonly DbSet<T> _dbSet;

        public RepositoryBase(RepositoryContext context)
        {
           
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public void Create(T entity) => _dbSet.Add(entity);

        public void Delete(T entity) => _dbSet.Remove(entity);


        public IQueryable<T> FindAll(bool trackChanges) =>
            !trackChanges ? _dbSet.AsNoTracking() :
            _dbSet;
        

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges)=>!trackChanges ? 
            _dbSet.Where(expression).AsNoTracking() :
            _dbSet.Where(expression);
       
        public void Update(T entity) => _dbSet.Update(entity);
        
    }
}
