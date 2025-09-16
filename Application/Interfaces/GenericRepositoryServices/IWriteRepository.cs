using Domain.BaseProjeEntities.CoreEntities;

namespace Application.Interfaces.GenericRepositoryServices;

public interface IWriteRepository<TEntity, TEntityId>
where TEntity : Entity<TEntityId>
{

    #region AddAsync
    ////////////////////////////////////// ADD ASYNC ///////////////////////////////////////////
    Task<TEntity> AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    #endregion

    #region AddRangeAsync
    ////////////////////////////////////// ADDRANGE ASYNC ///////////////////////////////////////////
    Task<ICollection<TEntity>> AddRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion

    #region UpdateAsync
    ////////////////////////////////////// UPDATE ASYNC ///////////////////////////////////////////
    Task<TEntity> UpdateAsync(TEntity entity, CancellationToken cancellationToken = default);
    #endregion

    #region UpdateRangeAsync
    ////////////////////////////////////// UPDATERANGE ASYNC ///////////////////////////////////////////
    Task<ICollection<TEntity>> UpdateRangeAsync(ICollection<TEntity> entities, CancellationToken cancellationToken = default);
    #endregion

    #region DeleteAsync

    ////////////////////////////////////// DELETE ASYNC ///////////////////////////////////////////
    Task<TEntity> DeleteAsync(TEntity entity, bool permanent = false, CancellationToken cancellationToken = default);
    #endregion

    #region DeleteRangeAsync
    ////////////////////////////////////// DELETERANGE ASYNC ///////////////////////////////////////////
    Task<ICollection<TEntity>> DeleteRangeAsync(
        ICollection<TEntity> entities,
        bool permanent = false,
        CancellationToken cancellationToken = default
    );
    #endregion


    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    Task SaveChangesAsync();

    #region Add
    TEntity Add(TEntity entity);
    #endregion

    #region AddRange
    ICollection<TEntity> AddRange(ICollection<TEntity> entities);
    #endregion

    #region Update
    TEntity Update(TEntity entity);
    #endregion

    #region UpdateRange
    ICollection<TEntity> UpdateRange(ICollection<TEntity> entities);
    #endregion

    #region Delete
    TEntity Delete(TEntity entity, bool permanent = false);
    #endregion

    #region DeleteRange
    ICollection<TEntity> DeleteRange(ICollection<TEntity> entities, bool permanent = false);
    #endregion
}
