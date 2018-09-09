using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace QTrans.Contract
{
    public interface IRepository<IEntity>
    {
        IQueryable<IEntity> GetAll();

        Task<IEntity> GetById(int id);

        ///IQueryable<IEntity> FindBy(Expression<Func<IEntity, bool>> predicate);
        Task Create(IEntity entity);

        Task Update(int id, IEntity entity);

        Task Delete(int id);
    }
}
