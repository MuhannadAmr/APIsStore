
using Microsoft.EntityFrameworkCore;
using Store.DEMO.Core;
using Store.DEMO.Core.Mapping.Products;
using Store.DEMO.Core.Services.Contract;
using Store.DEMO.Repository;
using Store.DEMO.Repository.Data.Contexts;
using Store.DEMO.Service.Services.Products;

namespace Store.DEMO.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreDbContext>(option =>
            {
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new ProductProfile()));

            var app = builder.Build();

            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var context = service.GetRequiredService<StoreDbContext>();
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There Are Problems during apply migrations!!");
            }

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
