using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Catalog
{
    public static class CatalogModule
    {
        public static IServiceCollection AddCatalogModule(this IServiceCollection services, IConfiguration configuration)
        {
            //Add services to the container

            //Api Endpoint services

            //Application use case services
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            //Data - Infractructure services
            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<CatalogDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseNpgsql(connectionString);
                });

            services.AddScoped<IDataSeeder, CatalogDataSeeder>();

            return services;
        }

        public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
        {
            //Configure the Http request pipeline

            //1. Use Api Endpoint services

            //2. Use Application Use case services

            //3. Use Data -  infrastructure services
            app.UseMigration<CatalogDbContext>();

            // InitialiseDatabaseAsync(app).GetAwaiter().GetResult();

            return app;
        }

        // private static async Task InitialiseDatabaseAsync(IApplicationBuilder app)
        // {
        //     using var scope = app.ApplicationServices.CreateScope();

        //     var context = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        //     await context.Database.MigrateAsync();
        // }
    }
}