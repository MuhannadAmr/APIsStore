using Store.DEMO.Core.Entites;
using Store.DEMO.Core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.DEMO.Core
{
    public interface IUnitOfWork
    {

        Task<int> CompleteAsync();

        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
