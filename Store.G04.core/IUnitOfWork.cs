using Store.G04.core.Entities;
using Store.G04.core.Repositories.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.G04.core
{
    public interface IUnitOfWork
    {
        Task<int> CompleteAsync();

        //Create Repository<T> and return
        IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;
    }
}
