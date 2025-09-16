using Domain.BaseProjeEntities.CoreEntities;
using Application.GenericRepositoryFiles.Common.DynamicQueryFilter;
using Application.GenericRepositoryFiles.Common.PagingFiles;
using Application.GenericRepositoryFiles.Common;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Application.Interfaces.GenericRepositoryServices;

public interface IReadRepository<TEntity, TEntityId> : IQuery<TEntity>
    where TEntity : Entity<TEntityId>
{
    ///////////////////////////////////////// Async ////////////////////////////////////////////////////////////

    #region GetAsync
    //////////////////////////////////////START GET ASYNC /////////////////////////////////////////////////
    Task<TEntity?> GetAsync(
           Expression<Func<TEntity, bool>> predicate,
           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
           bool withDeleted = false,
           bool enableTracking = true,
           CancellationToken cancellationToken = default
       );
    //////////////////////////////////////END GET ASYNC ///////////////////////////////////////////////////
    #endregion


    #region GetListAsync
    //////////////////////////////////////START GETLİST ASYNC ////////////////////////////////////////////
    Task<IPaginate<TEntity>> GetListAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    //////////////////////////////////////END GETLİST ASYNC /////////////////////////////////////////////
    #endregion 


    #region GetListByDynamicAsync
    //////////////////////////////////////START GETLİSTBYDYNAMİC ASYNC //////////////////////////////////

    Task<IPaginate<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    //////////////////////////////////////END GETLİSTBYDYNAMİC ASYNC ////////////////////////////////////
    #endregion


    #region AnyAsync
    //////////////////////////////////////START ANY ASYNC ///////////////////////////////////////////////
    Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default
    );

    //////////////////////////////////////END ANY ASYNC /////////////////////////////////////////////////
    #endregion


    #region GetAllByDynamicNoPagingAsync
    /////////////////////////////////////   GetAllNoPaginateByDynamicAsync   //////////////////////////////////////////////

    Task<List<TEntity>> GetAllByDynamicNoPagingAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    /////////////////////////////////////  GetAllNoPaginateAsync    //////////////////////////////////////////////
    #endregion


    #region GetAllNoPaginateAsync

    Task<List<TEntity>> GetAllNoPaginateAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    );
    #endregion

    ////////////////////////////////////////// NoAsync ////////////////////////////////////////////////////////

    #region Get
    TEntity? Get(
      Expression<Func<TEntity, bool>> predicate,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true
  );
    #endregion


    #region GetList
    IPaginate<TEntity> GetList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    );
    #endregion


    #region GetListByDynamic
    IPaginate<TEntity> GetListByDynamic(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    );
    #endregion


    #region Any
    bool Any(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false
    );
    #endregion


    #region GetAllByDynamicNoPaging
    List<TEntity> GetAllByDynamicNoPaging(
      DynamicQuery dynamic,
      Expression<Func<TEntity, bool>>? predicate = null,
      Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
      bool withDeleted = false,
      bool enableTracking = true
  );
    #endregion


    #region GetAllNoPaginate
    List<TEntity> GetAllNoPaginate(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        bool enableTracking = true
    );
    #endregion



    ///////////////////////////////////////////// END //////////////////////////////////////////////////////////
}