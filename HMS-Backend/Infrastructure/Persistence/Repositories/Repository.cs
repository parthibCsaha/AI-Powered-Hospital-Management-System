

using System.Linq.Expressions;
using HMS_Backend.Application.Interfaces;
using HMS_Backend.Domain.Common;
using Microsoft.EntityFrameworkCore;




namespace HMS_Backend.Infrastructure.Persistence.Repositories;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;
    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }
    
    public async Task<T?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbSet.FindAsync(new object[] { id }, ct);
    }

    public async Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbSet.ToListAsync(ct);
    }

    
    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.Where(predicate).ToListAsync(cancellationToken);

    public async Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken);
 
    public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbSet.AnyAsync(predicate, cancellationToken);
 
    public async Task<int> CountAsync(Expression<Func<T, bool>>? predicate = null, CancellationToken cancellationToken = default)
        => predicate is null
            ? await _dbSet.CountAsync(cancellationToken)
            : await _dbSet.CountAsync(predicate, cancellationToken);
 
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
    }

    public void SoftDelete(T entity)
    {
        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        Update(entity);
    }

    public void Delete(T entity)
    {
        _dbSet.Remove(entity);
    }

}


