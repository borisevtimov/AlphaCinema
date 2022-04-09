using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Infrastructure.Data.Common
{
    public class Repository : IRepository
    {
        private readonly DbContext dbContext;

        public Repository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IQueryable<T> All<T>() where T : class
        {
            return DbSet<T>().AsQueryable();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await dbContext.SaveChangesAsync();
        }

        public DbSet<T> DbSet<T>() where T : class
        {
            return dbContext.Set<T>();
        }

        public async Task AddAsync<T>(T entity) where T : class
        {
            await dbContext.AddAsync(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            dbContext.Remove(entity);
        }
    }
}
