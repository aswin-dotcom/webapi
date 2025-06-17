using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using webapi.Data;
using webapi.Models;
using webapi.Repository.IRepository;

namespace webapi.Repository
{
    public class Repository <T> : IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet  = _db.Set<T>();

        }
        public async Task Create(T entity)
        {
            await dbSet.AddAsync(entity);
            await save();

        }

        public async Task<T> Get(Expression<Func<T, bool>>? Filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query.AsNoTracking();

            }
            if (Filter != null)
            {
                query = query.Where(Filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? Filter = null)
        {
            IQueryable<T> query = dbSet;
            if (Filter != null)
            {
                query = query.Where(Filter);
            }
            return await query.ToListAsync();


        }

        public async Task Remove(T entity)
        {
            dbSet.Remove(entity);
            await save();
        }

        public async Task save()
        {
            await _db.SaveChangesAsync();
        }

        public async Task Update(T entity)
        {
            dbSet.Update(entity);
            await save();
        }
    }
}
