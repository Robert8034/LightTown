using System.Collections.Generic;
using System.Linq;

namespace LightTown.Core.Data
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        IQueryable<TEntity> Table { get; }
        IQueryable<TEntity> TableNoTracking { get; }

        TEntity GetById(object id);
        TEntity Insert(TEntity entity);
        void Insert(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        void Update(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void Delete(IEnumerable<TEntity> entities);
    }

}