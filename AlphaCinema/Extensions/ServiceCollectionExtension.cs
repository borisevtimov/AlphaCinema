using AlphaCinema.Core.Contracts;
using AlphaCinema.Core.Services;
using AlphaCinema.Infrastructure.Data;
using AlphaCinema.Infrastructure.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IRepository, Repository>()
                    .AddScoped<IAdminUserService, AdminUserService>()
                    .AddScoped<IMovieService, MovieService>()
                    .AddScoped<ITicketService, TicketService>();

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
