using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Data;

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

        // ���������� ��������� MVC
        services.AddControllersWithViews();

        // ���������� ��������� ������
        services.AddDistributedMemoryCache();
        services.AddSession();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
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
