using Store.DEMO.Core;
using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Repositories.Contract;
using Store.DEMO.Repository.Data.Contexts;
using Store.DEMO.Repository.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        private readonly Hashtable _repositories;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new Hashtable();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            var type = typeof(TEntity).Name;
            if(!_repositories.ContainsKey(type))
            {
                var repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(type, repository);
            }
            return _repositories[type] as IGenericRepository<TEntity, TKey>;
        }
    }
}
