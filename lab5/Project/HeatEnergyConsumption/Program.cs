using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Services.CacheService;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        IServiceCollection services = builder.Services;

        // ��������� ����������� ��� ������� � �� � �������������� EF
        string connectionDb = builder.Configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<HeatEnergyConsumptionContext>(options =>
            options.UseSqlServer(connectionDb));

        // ���������� ��������� Identity
        string connectionUsers = builder.Configuration.GetConnectionString("IdentityConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionUsers));
        services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultUI()
                .AddDefaultTokenProviders();

        services.AddControllersWithViews();

        // ���������� ��������� ������
        services.AddDistributedMemoryCache();
        services.AddSession();

        // ���������� ��������� �����������
        services.AddMemoryCache();
        services.AddScoped<OwnershipFormsCacheService>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.MapRazorPages();

        app.Run();
    }
}