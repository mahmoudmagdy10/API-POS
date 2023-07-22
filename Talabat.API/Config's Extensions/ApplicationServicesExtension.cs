using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.API.Mapping;
using Talabat.Core;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Errors;
using Talabat.Core.IRepository;
using Talabat.Core.IService;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;
using Talabat.Repository.Repositories;
using Talabat.Services;

namespace Talabat.API.Config_s_Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            //builder.Services.AddScoped(IGenericRep<Product>,GenericRep<Product>);
            services.AddScoped(typeof(IGenericRep<>), typeof(GenericRep<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddAutoMapper(typeof(MappingProfiles));

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)
                                                         .SelectMany(P => P.Value.Errors)
                                                         .Select(E => E.ErrorMessage)
                                                         .ToArray();

                    var ValidationErrorResponseNew = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };

                    return new BadRequestObjectResult(ValidationErrorResponseNew);
                };
            });

            return services;
        }

        public static async Task<WebApplication> AddMigrateAsyncMiddleware(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;

            var dbContext = services.GetRequiredService<StoreContext>();
            await dbContext.Database.MigrateAsync();
            await StoreContextSeed.SeedAsync(dbContext);

            var IdentityDbContext = services.GetRequiredService<AppIdentityDbContext>();
            await IdentityDbContext.Database.MigrateAsync();

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>(); // Ask CLR To Craete Obj From UserManager
            await AppIdentityDbContextSeed.SeedUserAsync(userManager);

            return app;
        }
    }
}
