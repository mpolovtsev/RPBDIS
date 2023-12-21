using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using WebAPI.Data;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            IServiceCollection services = builder.Services;

            // Внедрение зависимости для доступа к БД с использованием EF
            string connectionDb = builder.Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<HeatEnergyConsumptionContext>(options =>
                options.UseSqlServer(connectionDb));

            services.AddControllers();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Version = "v1",
                    Title = "Heat Energy Consumption API",
                    Description = "Данные об организациях",
                    Contact = new OpenApiContact
                    {
                        Name = "Polovtsev Maksim",
                        Url = new Uri("https://github.com/mpolovtsev/RPBDIS/tree/main/lab6/Project")
                    }
                });
            });

            var app = builder.Build();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heat Energy Consumption API V1");
            });

            app.UseDeveloperExceptionPage();
            app.UseDefaultFiles();
            app.UseStaticFiles();

            if (!app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseCors("AllowAll");
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}