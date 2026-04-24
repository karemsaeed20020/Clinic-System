
using Clinic_System.Data.Context;
using System.Linq.Expressions;
namespace Clinic_System.Core.Interfaces.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly AppDbContext context;

        public GenericRepository(AppDbContext context)
        {
            this.context = context;
        }
        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await context.Set<TEntity>().AddAsync(entity);
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>>? criteria = null, CancellationToken cancellationToken = default)
        {
            if (criteria != null)
            {
                return await context.Set<TEntity>().AsNoTracking().CountAsync(criteria);
            }

            return await context.Set<TEntity>().AsNoTracking().CountAsync();
        }

        public void Delete(TEntity enity, CancellationToken cancellationToken = default)
        {
            context.Set<TEntity>().Remove(enity);
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().AsNoTracking().Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().AsNoTracking().ToListAsync();
        }

        public async Task<TEntity?> GetByCondition(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        public async Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await context.Set<TEntity>().FindAsync(id);
        }

        public async Task<(IEnumerable<TEntity> Items, int TotalCount)> GetPaginatedAsync(int pageNumber, int pageSize, Expression<Func<TEntity, bool>>? filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = context.Set<TEntity>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            int totalCount = await query.CountAsync();
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return (items, totalCount);
        }

        public void SoftDelete(TEntity enity, CancellationToken cancellationToken = default)
        {
            if (enity is ISoftDelete softDeletableEntity)
            {
                var entry = context.Entry(enity);

                // الحل: إذا كانت الـ Entity متتبعة، نفصلها أولاً
                if (entry.State != EntityState.Detached)
                {
                    entry.State = EntityState.Detached;
                }

                // رفع الـ Entity أولاً (قبل تعديل القيم) - هذا يحفظ القيم الأصلية
                context.Set<TEntity>().Attach(enity);

                // تعديل القيم بعد Attach - هذا يضمن أن EF Core يكتشف التغييرات
                softDeletableEntity.IsDeleted = true;
                softDeletableEntity.DeletedAt = DateTime.Now;

                // تعيين الحالة إلى Modified يدوياً
                entry = context.Entry(enity);
                entry.State = EntityState.Modified;

                // التأكد من أن الخصائص محددة كـ Modified
                entry.Property(nameof(ISoftDelete.IsDeleted)).IsModified = true;
                entry.Property(nameof(ISoftDelete.DeletedAt)).IsModified = true;
            }
            else
            {
                throw new InvalidOperationException($"Entity of type {typeof(TEntity).Name} does not support Soft Delete.");
            }
        }

        public void Update(TEntity enity, CancellationToken cancellationToken = default)
        {
            var entry = context.Entry(enity);
            if (entry.State == EntityState.Detached)
            {
                context.Set<TEntity>().Update(enity);
            }
            else
            {
                entry.State = EntityState.Modified;
            }
        }
    }
}
