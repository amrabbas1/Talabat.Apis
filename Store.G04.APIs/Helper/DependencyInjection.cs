﻿using Microsoft.EntityFrameworkCore;
using Store.G04.core.Services.Contract;
using Store.G04.core;
using Store.G04.Repository;
using Store.G04.Repository.Data.Contexts;
using Store.G04.Service.Services.Products;
using Store.G04.core.Mapping.Products;
using Microsoft.AspNetCore.Mvc;
using Store.G04.APIs.Errors;
using Store.G04.core.Repositories.Contract;
using StackExchange.Redis;
using Store.G04.core.Mapping.Baskets;
using Store.G04.Repository.Repositories;
using Store.G04.Service.Services.Caches;
using Store.G04.Repository.Identity.Contexts;
using Store.G04.core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Store.G04.Service.Services.Tokens;
using Store.G04.Service.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Store.G04.core.Mapping.Auth;
using Store.G04.core.Mapping.Orders;
using Store.G04.Service.Services.Orders;
using Store.G04.Service.Services.Payments;

namespace Store.G04.APIs.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddBuiltInService();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddUserDefinedService();
            services.AddAutoMapperService(configuration);
            services.ConfigureInvalidModelStateResponseService();
            services.AddRedisService(configuration);
            services.AddIdentityService();
            services.AddAuthenticationService(configuration);

            return services;
        }
        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
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
        private static IServiceCollection AddDbContextService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            return services;
        }
        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IproductService, ProductService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IBasketRepository, BasketRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IPaymentService, PaymentService>();

            return services;
        }
        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new ProductProfile(configuration)));
            services.AddAutoMapper(M => M.AddProfile(new BasketProfile()));
            services.AddAutoMapper(M => M.AddProfile(new AuthProfile()));
            services.AddAutoMapper(M => M.AddProfile(new OrderProfile(configuration)));


            return services;
        }
        private static IServiceCollection ConfigureInvalidModelStateResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count() > 0)//if count > 0 => there is errors
                                            .SelectMany(P => P.Value.Errors)
                                            .Select(E => E.ErrorMessage)
                                            .ToArray();

                    var response = new ApiValidationErrorResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(response);
                };
            });

            return services;
        }
        private static IServiceCollection AddRedisService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IConnectionMultiplexer>((serviceProvider) =>
            {
                var connection = configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(connection);
            });
            return services;
        }
        private static IServiceCollection AddIdentityService(this IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<StoreIdentityDbContext>();
            return services;
        }
        private static IServiceCollection AddAuthenticationService(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))

                }
            );
            return services;
        }


    }
}
