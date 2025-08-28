using System;
using System.IO;
using AuthenticationApp.Data;
using AuthenticationApp.Services;
using AuthenticationApp.ViewModels;
using Avalonia.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AuthenticationApp.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddCommonServices(this IServiceCollection collection)
    {
        // Register applicationDbContext
        collection.AddDbContext<ApplicationDbContext>(options =>
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dbPath = Path.Combine(baseDirectory, "AuthenticationApp.db");
            options.UseSqlite($"Data Source={dbPath}");
        });
        
        // Register Services
        collection.AddSingleton<INavigationService, NavigationService>();
        collection.AddTransient<IAuthenticationService, AuthenticationService>();
        
        
        // Register View Models
        collection.AddTransient<MainWindowViewModel>();
        collection.AddTransient<LoginViewModel>();
        collection.AddTransient<RegisterViewModel>();
        collection.AddTransient<HomeViewModel>();
        
    }
}