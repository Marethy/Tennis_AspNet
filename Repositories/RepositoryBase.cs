﻿using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Tennis.Data;
using Tennis.Interfaces;

namespace Tennis.Repositories;

public class RepositoryBase<TEntity> : IRepository<TEntity> where TEntity : class
{
    protected TennisWebMVCContext _context;

    public RepositoryBase(TennisWebMVCContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _context.Set<TEntity>().AddAsync(entity);
    }

    public int Count(Expression<Func<TEntity, bool>> where)
    {
        return _context.Set<TEntity>().Count(where);
    }

    public void Delete(TEntity entity)
    {
        _context.Remove(entity);
    }

    public void Delete(Expression<Func<TEntity, bool>> where)
    {
        var list = _context.Set<TEntity>().Where(where).AsEnumerable();
        _context.RemoveRange(list);
    }

    public async Task<TEntity> GetByIdAsync(object id)
    {
        return await _context.Set<TEntity>().FindAsync(id);
    }

    public async Task<IEnumerable<TEntity>> GetListAsync(Expression<Func<TEntity, bool>> filter,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy, string includeProperties = "", int skip = 0,
        int take = 0)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>();
        if (filter != null) query = query.Where(filter);
        foreach (var includeProperty in includeProperties.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            query = query.Include(includeProperty);
        if (skip > 0) query = query.Skip(skip);
        if (take > 0) query = query.Take(take);
        if (orderBy != null) return await orderBy(query).ToListAsync();

        return await query.ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> filter)
    {
        return await _context.Set<TEntity>().AnyAsync(filter);
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(TEntity entity)
    {
        _context.Update(entity);
    }
}
