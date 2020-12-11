using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpacedOut.Infrastucture.Data;

namespace SpacedOut.Infrastucture
{
    public static class StartupSetup
    {
        // will be created in web project root
        public static void AddDbContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
        }
    }
}