
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.G04.APIs.Errors;
using Store.G04.APIs.MiddleWares;
using Store.G04.core;
using Store.G04.core.Mapping.Products;
using Store.G04.core.Services.Contract;
using Store.G04.Repository;
using Store.G04.Repository.Data.Contexts;
using Store.G04.Service.Services.Products;
using Store.G04.APIs.Helper;
namespace Store.G04.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDependency(builder.Configuration);

            var app = builder.Build();

            await app.ConfigureMiddleWareAsync();

            app.Run();
        }
    }
}
