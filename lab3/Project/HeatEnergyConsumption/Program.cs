using Microsoft.EntityFrameworkCore;
using HeatEnergyConsumption.Services.CacheService;
using HeatEnergyConsumption.Data;
using HeatEnergyConsumption.Models;
using HeatEnergyConsumption.HTML;
using HeatEnergyConsumption.Infrastructure;

namespace HeatEnergyConsumption
{
    public class Program
    {
        public static void Main()
        {
            var builder = WebApplication.CreateBuilder();

            var services = builder.Services;
            // ���������� ������� � ���� ������ � �������������� EF
            services.AddDbContext<HeatEnergyConsumptionContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

            // ���������� �����������
            services.AddMemoryCache();
            services.AddScoped<ChiefPowerEngineersCacheService>();
            services.AddScoped<HeatEnergyConsumptionRatesCacheService>();
            services.AddScoped<ManagersCacheService>();
            services.AddScoped<OrganizationsCacheService>();
            services.AddScoped<OwnershipFormsCacheService>();
            services.AddScoped<ProducedProductsCacheService>();
            services.AddScoped<ProductsTypesCacheService>();
            services.AddScoped<ProvidedServicesCacheService>();
            services.AddScoped<ServicesTypesCacheService>();

            // ���������� ��������� ������
            services.AddDistributedMemoryCache();
            services.AddSession();

            var app = builder.Build();

            app.UseSession();

            // �������� � ����������� � �������
            app.Map("/info", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    htmlBuilder.OpenHtml();
                    htmlBuilder.OpenHead();
                    htmlBuilder.CreateTitle("���������� � �������");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("���������� � �������", 1, "center");
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("������: ");
                    htmlBuilder.AddText($"{context.Request.Host}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("����: ");
                    htmlBuilder.AddText($"{context.Request.PathBase}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("��������: ");
                    htmlBuilder.AddText($"{context.Request.Protocol}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("�����: ");
                    htmlBuilder.AddText($"{context.Request.Method}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("�������", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� ChiefPowerEngineers
            app.Map("/chief_power_engineers", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ChiefPowerEngineersCacheService cacheService =
                    services.GetService<ChiefPowerEngineersCacheService>();
                    IEnumerable<ChiefPowerEngineer> records =
                    cacheService.Get("ChiefPowerEngineers20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "���", "�������", "��������", "����� ��������", "ID �����������" };
                    htmlBuilder.CreatePageWithTable("���������� � ������� �����������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� HeatEnergyConsumptionRates
            app.Map("/heat_energy_consumption_rates", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    HeatEnergyConsumptionRatesCacheService cacheService =
                    services.GetService<HeatEnergyConsumptionRatesCacheService>();
                    IEnumerable<HeatEnergyConsumptionRate> records =
                    cacheService.Get("HeatEnergyConsumptionRates20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "ID �����������", "ID ���� ���������", "����������", "����" };
                    htmlBuilder.CreatePageWithTable("���������� � ������ ������� ������������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� Managers
            app.Map("/managers", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ManagersCacheService cacheService =
                    services.GetService<ManagersCacheService>();
                    IEnumerable<Manager> records =
                    cacheService.Get("Managers20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "���", "�������", "��������", "����� ��������" };
                    htmlBuilder.CreatePageWithTable("���������� � �������������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� Organizations
            app.Map("/organizations", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    OrganizationsCacheService cacheService =
                    services.GetService<OrganizationsCacheService>();
                    IEnumerable<Organization> records =
                    cacheService.Get("Organizations20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "��������", "ID ����� �����������", "�����", "ID ������������" };
                    htmlBuilder.CreatePageWithTable("���������� �� ������������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� OwnershipForms
            app.Map("/ownership_forms", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    OwnershipFormsCacheService cacheService =
                    services.GetService<OwnershipFormsCacheService>();
                    IEnumerable<OwnershipForm> records =
                    cacheService.Get("OwnershipForms20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "��������" };
                    htmlBuilder.CreatePageWithTable("���������� � ������ �������������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� ProducedProducts
            app.Map("/produced_products", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ProducedProductsCacheService cacheService =
                    services.GetService<ProducedProductsCacheService>();
                    IEnumerable<ProducedProduct> records =
                    cacheService.Get("ProducedProducts20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "ID �����������", "ID ���� ���������", "���������� ���������", 
                        "���������� ����������� ������������", "����" };
                    htmlBuilder.CreatePageWithTable("���������� � ������������� ���������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� ProductsTypes
            app.Map("/products_types", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ProductsTypesCacheService cacheService =
                    services.GetService<ProductsTypesCacheService>();
                    IEnumerable<ProductsType> records =
                    cacheService.Get("ProductsTypes20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "���", "��������", "������� ���������" };
                    htmlBuilder.CreatePageWithTable("���������� � ����� ���������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� ProvidedServices
            app.Map("/provided_services", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ProvidedServicesCacheService cacheService =
                    services.GetService<ProvidedServicesCacheService>();
                    IEnumerable<ProvidedService> records =
                    cacheService.Get("ProvidedServices20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "ID �����������", "ID ���� ������", "����������", "����" };
                    htmlBuilder.CreatePageWithTable("���������� �� ��������� �������", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ����������� �� ������� ServicesTypes
            app.Map("/services_types", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ServicesTypesCacheService cacheService =
                    services.GetService<ServicesTypesCacheService>();
                    IEnumerable<ServicesType> records =
                    cacheService.Get("ServicesTypes20");

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    string[] headers = { "ID", "���", "��������", "������� ���������" };
                    htmlBuilder.CreatePageWithTable("���������� � ����� �����", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ������ ��� ������ ������� � ������� ProducedProducts � �������������� cookies
            app.Map("/search_form1", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ProducedProductsCacheService cacheService =
                    services.GetService<ProducedProductsCacheService>();
                    IEnumerable<ProducedProduct> records =
                    cacheService.Get("ProducedProducts20");

                    context.Request.Cookies.TryGetValue("ProductsQuantity", out string? productsQuantity);

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    htmlBuilder.OpenHtml();
                    htmlBuilder.OpenHead();
                    htmlBuilder.CreateTitle("����� ������� � ������� ProducedProducts");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("����� ������� � ������� ProducedProducts, ��� ������� " +
                        "���������� ��������� ������ ���������", 1, "center");
                    htmlBuilder.OpenForm("post", "/search_form1");
                    htmlBuilder.OpenLabel();
                    htmlBuilder.AddText("������� ���������� ���������:  ");
                    htmlBuilder.CreateInput("ProductsQuantity", "number", productsQuantity == null ? "" : productsQuantity);
                    htmlBuilder.CloseLabel();
                    htmlBuilder.CreateInput("SearchButton", "submit", "�����");
                    htmlBuilder.CloseForm();
                    htmlBuilder.CreateBr();

                    if (context.Request.Method == "POST")
                    {
                        productsQuantity = context.Request.Form["ProductsQuantity"];
                        context.Response.Cookies.Append("ProductsQuantity", productsQuantity);

                        IEnumerable<ProducedProduct> filteredRecords = records.Where(record => 
                        record.ProductQuantity < int.Parse(productsQuantity));

                        if (filteredRecords.Count() == 0)
                            htmlBuilder.CreateH("������, ��������������� �������, �� �������.", 1, "center");
                        else
                        {
                            string[] headers = { "ID", "ID �����������", "ID ���� ���������", "���������� ���������", 
                                "���������� ����������� ������������", "����" };
                            htmlBuilder.CreateTable(headers, filteredRecords.ToList());
                        }
                    }

                    htmlBuilder.CreateBr();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("�������", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // �������� � ������ ��� ������ ������� � ������� ProducedProducts � �������������� sessions
            app.Map("/search_form2", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    var services = context.RequestServices;
                    ProducedProductsCacheService cacheService =
                    services.GetService<ProducedProductsCacheService>();
                    IEnumerable<ProducedProduct> records =
                    cacheService.Get("ProducedProducts20");
                     
                    ProducedProduct producedProduct = context.Session.Get<ProducedProduct>("ProducedProduct") ?? new ProducedProduct();

                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    htmlBuilder.OpenHtml();
                    htmlBuilder.OpenHead();
                    htmlBuilder.CreateTitle("����� ������� � ������� ProducedProducts");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("����� ������� � ������� ProducedProducts, ��� ������� " +
                        "���������� ��������� ������ ���������", 1, "center");
                    htmlBuilder.OpenForm("post", "/search_form2");
                    htmlBuilder.OpenLabel();
                    htmlBuilder.AddText("������� ���������� ���������:  ");
                    htmlBuilder.CreateInput("ProductsQuantity", "number", 
                        producedProduct.ProductQuantity == 0 ? "" : producedProduct.ProductQuantity.ToString());
                    htmlBuilder.CloseLabel();
                    htmlBuilder.CreateInput("SearchButton", "submit", "�����");
                    htmlBuilder.CloseForm();
                    htmlBuilder.CreateBr();

                    if (context.Request.Method == "POST")
                    {
                        string productsQuantity = context.Request.Form["ProductsQuantity"];
                        producedProduct.ProductQuantity = int.Parse(productsQuantity);
                        context.Session.Set("ProducedProduct", producedProduct);

                        IEnumerable<ProducedProduct> filteredRecords = records.Where(record => 
                        record.ProductQuantity < int.Parse(productsQuantity));

                        if (filteredRecords.Count() == 0)
                            htmlBuilder.CreateH("������, ��������������� �������, �� �������.");
                        else
                        {
                            string[] headers = { "ID", "ID �����������", "ID ���� ���������", "���������� ���������", 
                                "���������� ����������� ������������", "����" };
                            htmlBuilder.CreateTable(headers, filteredRecords.ToList());
                        }
                    }

                    htmlBuilder.CreateBr();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("�������", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // ������� ��������
            app.Run(async context =>
            {
                var services = context.RequestServices;
                ChiefPowerEngineersCacheService chiefPowerEngineersCacheService =
                services.GetService<ChiefPowerEngineersCacheService>();
                HeatEnergyConsumptionRatesCacheService heatEnergyConsumptionRatesCacheService =
                services.GetService<HeatEnergyConsumptionRatesCacheService>();
                ManagersCacheService managersCacheService =
                services.GetService<ManagersCacheService>();
                OrganizationsCacheService organizationsCacheService =
                services.GetService<OrganizationsCacheService>();
                OwnershipFormsCacheService ownershipFormsCacheService =
                services.GetService<OwnershipFormsCacheService>();
                ProducedProductsCacheService producedProductsCacheService =
                services.GetService<ProducedProductsCacheService>();
                ProductsTypesCacheService productsTypesCacheService =
                services.GetService<ProductsTypesCacheService>();
                ProvidedServicesCacheService providedServicesCacheService =
                services.GetService<ProvidedServicesCacheService>();
                ServicesTypesCacheService servicesTypesCacheService =
                services.GetService<ServicesTypesCacheService>();

                chiefPowerEngineersCacheService.Add("ChiefPowerEngineers20");
                heatEnergyConsumptionRatesCacheService.Add("HeatEnergyConsumptionRates20");
                managersCacheService.Add("Managers20");
                organizationsCacheService.Add("Organizations20");
                ownershipFormsCacheService.Add("OwnershipForms20");
                producedProductsCacheService.Add("ProducedProducts20");
                productsTypesCacheService.Add("ProductsTypes20");
                providedServicesCacheService.Add("ProvidedServices20");
                servicesTypesCacheService.Add("ServicesTypes20");

                HTMLBuilder htmlBuilder = new HTMLBuilder();
                htmlBuilder.OpenHtml();
                htmlBuilder.OpenHead();
                htmlBuilder.CreateTitle("������� ��������");
                htmlBuilder.CloseHead();
                htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                htmlBuilder.OpenBody();
                htmlBuilder.CreateH("����", 1, "center");
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("���������� � �������", "/info");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ������� �����������", "/chief_power_engineers");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ������ ������� ������������", "/heat_energy_consumption_rates");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � �������������", "/managers");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� �� ������������", "/organizations");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ������ �������������", "/ownership_forms");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ������������� ���������", "/produced_products");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ����� ���������", "/products_types");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� �� ��������� �������", "/provided_services");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("������� � ����������� � ����� �����", "/services_types");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("����� ��� ������ ������������� ��������� � �������������� cookies", "/search_form1");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("����� ��� ������ ������������� ��������� � �������������� sessions", "/search_form2");
                htmlBuilder.CloseDiv();
                htmlBuilder.CloseBody();
                htmlBuilder.CloseHtml();

                await context.Response.WriteAsync(htmlBuilder.Code);
            });

            app.Run();
        }
    }
}