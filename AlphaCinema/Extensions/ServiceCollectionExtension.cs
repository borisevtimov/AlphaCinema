using AlphaCinema.Core.Contracts.Admin;
using AlphaCinema.Core.Services.Admin;
using AlphaCinema.Infrastructure.Data;
using AlphaCinema.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services) 
        {
            services.AddScoped<IRepository, Repository>();
            services.AddScoped<IAdminUserService, AdminUserService>();

            return services;
        }

        public static IServiceCollection AddApplicationDbContexts(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddDatabaseDeveloperPageExceptionFilter();

            return services;
        }
    }
}
