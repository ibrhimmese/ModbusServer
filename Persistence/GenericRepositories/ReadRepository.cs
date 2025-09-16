using Application.Interfaces.GenericRepositoryServices;
using Domain.BaseProjeEntities.CoreEntities;
using Application.GenericRepositoryFiles.Common.DynamicQueryFilter;
using Application.GenericRepositoryFiles.Common.PagingFiles;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Persistence.GenericRepositories;

public class ReadRepository<TEntity, TEntityId, TContext> : IReadRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TContext : DbContext
{
    protected readonly TContext Context;

    public ReadRepository(TContext context)
    {
        Context = context;
    }

    public IQueryable<TEntity> Query()
    {
        return Context.Set<TEntity>();
    }

    public async Task<IPaginate<TEntity>> GetListAsync(
       Expression<Func<TEntity, bool>>? predicate = null,
       Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       int index = 0,
       int size = 10,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToPaginateAsync(index, size, from: 0, cancellationToken);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }


    public async Task<TEntity?> GetAsync(
       Expression<Func<TEntity, bool>> predicate,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true,
       CancellationToken cancellationToken = default
   )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return await queryable.FirstOrDefaultAsync(predicate, cancellationToken);
    }

    public async Task<IPaginate<TEntity>> GetListByDynamicAsync(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.ToPaginateAsync(index, size, from: 0, cancellationToken);
    }



    public async Task<List<TEntity>> GetAllByDynamicNoPagingAsync(
 DynamicQuery dynamic,
 Expression<Func<TEntity, bool>>? predicate = null,
 Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
 bool withDeleted = false,
 bool enableTracking = true,
 CancellationToken cancellationToken = default
)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);

        return await queryable.ToListAsync(cancellationToken);
    }

    public async Task<List<TEntity>> GetAllNoPaginateAsync(
     Expression<Func<TEntity, bool>>? predicate = null,
     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
     bool withDeleted = false,
     bool enableTracking = true,
     CancellationToken cancellationToken = default
 )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return await orderBy(queryable).ToListAsync(cancellationToken);
        return await queryable.ToListAsync(cancellationToken);
    }


    public async Task<bool> AnyAsync(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<TEntity> queryable = Query();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return await queryable.AnyAsync(cancellationToken);
    }


    /////////////////////////////////////////////////////////////////////


    public TEntity? Get(
       Expression<Func<TEntity, bool>> predicate,
       Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
       bool withDeleted = false,
       bool enableTracking = true
   )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        return queryable.FirstOrDefault(predicate);
    }

    public IPaginate<TEntity> GetList(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable).ToPaginate(index, size);
        return queryable.ToPaginate(index, size);
    }

    public IPaginate<TEntity> GetListByDynamic(
        DynamicQuery dynamic,
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        int index = 0,
        int size = 10,
        bool withDeleted = false,
        bool enableTracking = true
    )
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.ToPaginate(index, size);
    }

    public bool Any(
        Expression<Func<TEntity, bool>>? predicate = null,
        Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
        bool withDeleted = false
    )
    {
        IQueryable<TEntity> queryable = Query();
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        return queryable.Any();
    }


    public List<TEntity> GetAllByDynamicNoPaging(
    DynamicQuery dynamic,
Expression<Func<TEntity, bool>>? predicate = null,
Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
bool withDeleted = false,
bool enableTracking = true
)
    {
        IQueryable<TEntity> queryable = Query().ToDynamic(dynamic);
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);

        return queryable.ToList();
    }

    public List<TEntity> GetAllNoPaginate(
     Expression<Func<TEntity, bool>>? predicate = null,
     Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
     Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>>? include = null,
     bool withDeleted = false,
     bool enableTracking = true
 )
    {
        IQueryable<TEntity> queryable = Query();
        if (!enableTracking)
            queryable = queryable.AsNoTracking();
        if (include != null)
            queryable = include(queryable);
        if (withDeleted)
            queryable = queryable.IgnoreQueryFilters();
        if (predicate != null)
            queryable = queryable.Where(predicate);
        if (orderBy != null)
            return orderBy(queryable).ToList();
        return queryable.ToList();
    }

}