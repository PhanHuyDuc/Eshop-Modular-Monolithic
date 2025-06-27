using Basket.Data.Processors;
using Shared.Data;

namespace Basket
{
    public static class BasketModule
    {
        public static IServiceCollection AddBasketModule(this IServiceCollection services, IConfiguration configuration)
        {
            //Add services to the container

            //1. Api Endpointservices

            //2. Application use case services
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.Decorate<IBasketRepository, CachedBasketRepository>();

            //3. Data - Infrastructure services

            var connectionString = configuration.GetConnectionString("Database");

            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

            services.AddDbContext<BasketDbContext>((sp, options) =>
                {
                    options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
                    options.UseNpgsql(connectionString);
                });
            services.AddHostedService<OutboxProcessor>();
            return services;
        }
        public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
        {
            // Configuration the Http request pipeline

            //1. Api Endpointservices

            //2. Application use case services


            //3. Data - Infrastructure services

            app.UseMigration<BasketDbContext>();
            return app;
        }
    }
}