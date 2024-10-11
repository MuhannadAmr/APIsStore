
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Store.DEMO.APIs.Errors;
using Store.DEMO.APIs.Helper;
using Store.DEMO.APIs.MiddleWares;
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

            builder.Services.AddDependency(builder.Configuration);
            
            var app = builder.Build();

            await app.ConfigureMiddlewareAsync();

            app.Run();
        }
    }
}
