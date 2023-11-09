using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Middleware;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        // Внедрение зависимости для доступа к БД с использованием EF
        string connection = builder.Configuration.GetConnectionString("ConnectionString");
        services.AddDbContext<HeatEnergyConsumptionContext>(options => options.UseSqlServer(connection));

        // Добавление поддержки сессии
        services.AddDistributedMemoryCache();
        services.AddSession();

        // Добавление функциональности MVC
        services.AddControllersWithViews(options =>
        {
            options.CacheProfiles.Add("Caching",
                new CacheProfile()
                {
                    Duration = 2 * 24 + 240
                });
        });

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
            app.UseExceptionHandler("/Home/Error");

        // Добавление поддержки статических файлов
        app.UseStaticFiles();

        // Добавление поддержки сессий
        app.UseSession();

        // Добавление компонента middleware для инициализации БД
        app.UseDbInitializer();

        // Добавление поддержки системы маршрутизации
        app.UseRouting();

		app.MapControllerRoute(
	        name: "default",
	        pattern: "{controller=Home}/{action=Index}/{id?}");

		app.MapControllerRoute(
            name: "ownershipForms",
            pattern: "{controller=OwnershipForms}/{action=Table}");

		app.MapControllerRoute(
	        name: "managers",
	        pattern: "{controller=Managers}/{action=Table}");

		app.MapControllerRoute(
	        name: "organizations",
	        pattern: "{controller=Organizations}/{action=Table}");

		app.Run();
    }
}