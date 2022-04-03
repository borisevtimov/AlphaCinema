using Microsoft.EntityFrameworkCore;

namespace AlphaCinema.Infrastructure.Data.Common
{
    public interface IRepository
    {
        Task<int> SaveChangesAsync();

        IQueryable<T> All<T>() where T : class;

        Task Add<T>(T entity) where T : class;

        void Delete<T>(T entity) where T : class;
    }
}
