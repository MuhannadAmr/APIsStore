using Store.DEMO.APIs.MiddleWares;
using Store.DEMO.Repository.Data.Contexts;
using Store.DEMO.Repository;
using Microsoft.EntityFrameworkCore;
using Store.DEMO.Repository.Identity.Contexts;
using Store.DEMO.Repository.Identity;
using Microsoft.AspNetCore.Identity;
using Store.DEMO.Core.Entites.Identity;

namespace Store.DEMO.APIs.Helper
{
    public static class ConfigureMiddleWare
    {
        public static async Task<WebApplication> ConfigureMiddlewareAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var service = scope.ServiceProvider;
            var context = service.GetRequiredService<StoreDbContext>();
            var identityContext = service.GetRequiredService<StoreIdentityDbContext>();
            var userManager = service.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = service.GetRequiredService<ILoggerFactory>();

            try
            {
                await context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);

                await identityContext.Database.MigrateAsync();
                await StoreIdentityDbContextSeed.SeedAppUserAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = loggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "There Are Problems during apply migrations!!");
            }

            app.UseMiddleware<ExceptionMiddleWare>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");

            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            return app;
        }
    }
}
