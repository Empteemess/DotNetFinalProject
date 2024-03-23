using Core.Services.Services;
using FinalProject.Data;
using FinalProject.Interfaces;
using FinalProject.Models;
using FinalProject.Repositories;
using FinalProject.Services;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Configurations;

public static class AppServices
{
    public static void AddServiceCollections(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddScoped<IHomeRepository<Product>, HomeRepository>();
        serviceCollection.AddScoped<IHomeService, HomeService>();
        
        serviceCollection.AddScoped<ISingleProductRepository<Product>, SingleProductRepository>();
        serviceCollection.AddScoped<ISingleProductService, SingleProductService>();

        serviceCollection.AddScoped<IShoppingService, ShoppingCartService>();

        serviceCollection.AddScoped<IAccountService, AccountService>();

        serviceCollection.AddScoped<IAdminRepository<Product>, AdminRepository>();
        serviceCollection.AddScoped<IAdminService, AdminService>();
        
        serviceCollection.AddScoped<IProductRepository<Product>, ProductRepository>();
        serviceCollection.AddScoped<IProductService, ProductService>();
        

        serviceCollection.AddTransient<DependencyConfiguration>();
        serviceCollection.AddTransient<AccountService>();
    }

    public static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<AppDbContext>()
            .AddDefaultTokenProviders();
    }
}