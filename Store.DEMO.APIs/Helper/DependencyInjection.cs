using Microsoft.EntityFrameworkCore;
using Store.DEMO.Core.Mapping.Products;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Core;
using Store.DEMO.Repository;
using Store.DEMO.Repository.Data.Contexts;
using Store.DEMO.Service.Services.Products;
using Microsoft.AspNetCore.Mvc;
using Store.DEMO.APIs.Errors;
using Microsoft.Extensions.Configuration;
using Store.DEMO.Core.Repositories.Contract;
using Store.DEMO.Repository.Repositories;
using StackExchange.Redis;
using Store.DEMO.Core.Mapping.Baskets;

namespace Store.DEMO.APIs.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddAutoMapperService(configuration);
            services.AddUserDefinedService();
            services.ConfigureInvalidModelStateResponseService();
            services.AddRadisService(configuration);
            return services;
        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers();
            return services;
        }
        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            return services;
        }
        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));

            return services;
        }
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(option =>
            {
                option.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(P => P.ErrorMessage).ToArray();
                    var res = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(res);
                };
            });

            return services;
        }
        private static IServiceCollection AddRadisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connect = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connect);
            });
            return services;
        }



    }
}
