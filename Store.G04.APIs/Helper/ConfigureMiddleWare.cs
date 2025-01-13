using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Store.G04.APIs.MiddleWares;
using Store.G04.core;
using Store.G04.core.Entities.Identity;
using Store.G04.Repository.Data.Contexts;
using Store.G04.Repository.Identity;
using Store.G04.Repository.Identity.Contexts;

namespace Store.G04.APIs.Helper
{
    public static class ConfigureMiddleWare
    {
        public static async Task<WebApplication> ConfigureMiddleWareAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var services = scope.ServiceProvider;

            var context = services.GetRequiredService<StoreDbContext>();
            var identityContext = services.GetRequiredService<StoreIdentityDbContext>();
            var userManager = services.GetRequiredService<UserManager<AppUser>>();
            var loggerFactory = services.GetRequiredService<ILoggerFactory>();

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
                logger.LogError(ex, "There are problems during apply migrations !");
            }


            app.UseMiddleware<ExceptionMiddleWare>();//Configure User-Defined[ExceptionMiddleWare] Middleware

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStatusCodePagesWithReExecute("/error/{0}");//lma b3ml call endpoint msh mwgoda byroh 3la
                                                              //el end point ele el path bta3ha : "/error/{0}"
                                                              //ele mwgoda fe ErrorController
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            return app;

        }

    }
}
