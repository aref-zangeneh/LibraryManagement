using LibraryManagement.Domain.Entities.Common;
using System.Linq.Expressions;

namespace LibraryManagement.Application.Interfaces.Repositories;
public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    #region Signitures
    /// <summary>
    /// get all entities of type TEntity.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> GetAllAsync();

    /// <summary>
    /// get entity by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<TEntity?> GetByIdAsync(int id);

    /// <summary>
    /// find entities by predicate.
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate);

    /// <summary>
    /// add a new entity of type TEntity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task AddAsync(TEntity entity);

    /// <summary>
    ///update an existing entity of type TEntity.
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    Task UpdateAsync(TEntity entity);

    /// <summary>
    ///soft delete entity by id 
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task SoftDeleteAsync(int id);


    /// <summary>
    /// hard delete entity by id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task HardDeleteAsync(int id);

    #endregion
}

