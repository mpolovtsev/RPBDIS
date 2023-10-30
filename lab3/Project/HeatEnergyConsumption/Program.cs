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
            // Добавление доступа к базе данных с использованием EF
            services.AddDbContext<HeatEnergyConsumptionContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString")));

            // Добавление кеширования
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

            // Добавление поддержки сессий
            services.AddDistributedMemoryCache();
            services.AddSession();

            var app = builder.Build();

            app.UseSession();

            // Страница с информацией о клиенте
            app.Map("/info", appBuilder =>
            {
                appBuilder.Run(async context =>
                {
                    HTMLBuilder htmlBuilder = new HTMLBuilder();
                    htmlBuilder.OpenHtml();
                    htmlBuilder.OpenHead();
                    htmlBuilder.CreateTitle("Информация о клиенте");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("Информация о клиенте", 1, "center");
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("Сервер: ");
                    htmlBuilder.AddText($"{context.Request.Host}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("Путь: ");
                    htmlBuilder.AddText($"{context.Request.PathBase}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("Протокол: ");
                    htmlBuilder.AddText($"{context.Request.Protocol}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenP();
                    htmlBuilder.CreateB("Метод: ");
                    htmlBuilder.AddText($"{context.Request.Method}.");
                    htmlBuilder.CloseP();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("Главная", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы ChiefPowerEngineers
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
                    string[] headers = { "ID", "Имя", "Фамилия", "Отчество", "Номер телефона", "ID организации" };
                    htmlBuilder.CreatePageWithTable("Информация о главных энергетиках", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы HeatEnergyConsumptionRates
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
                    string[] headers = { "ID", "ID организации", "ID типа продукции", "Количество", "Дата" };
                    htmlBuilder.CreatePageWithTable("Информация о нормах расхода теплоэнергии", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы Managers
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
                    string[] headers = { "ID", "Имя", "Фамилия", "Отчество", "Номер телефона" };
                    htmlBuilder.CreatePageWithTable("Информация о руководителях", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы Organizations
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
                    string[] headers = { "ID", "Название", "ID формы организации", "Адрес", "ID руководителя" };
                    htmlBuilder.CreatePageWithTable("Информация об организациях", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы OwnershipForms
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
                    string[] headers = { "ID", "Название" };
                    htmlBuilder.CreatePageWithTable("Информация о формах собственности", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы ProducedProducts
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
                    string[] headers = { "ID", "ID организации", "ID вида продукции", "Количество продукции", 
                        "Количество затраченной теплоэнергии", "Дата" };
                    htmlBuilder.CreatePageWithTable("Информация о произведенной продукции", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы ProductsTypes
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
                    string[] headers = { "ID", "Код", "Название", "Единица измерения" };
                    htmlBuilder.CreatePageWithTable("Информация о типах продукции", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы ProvidedServices
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
                    string[] headers = { "ID", "ID организации", "ID вида услуги", "Количество", "Дата" };
                    htmlBuilder.CreatePageWithTable("Информация об оказанных услугах", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с информацией из таблицы ServicesTypes
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
                    string[] headers = { "ID", "Код", "Название", "Единица измерения" };
                    htmlBuilder.CreatePageWithTable("Информация о типах услуг", headers, records);

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с формой для поиска записей в таблице ProducedProducts с использованием cookies
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
                    htmlBuilder.CreateTitle("Поиск записей в таблице ProducedProducts");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("Поиск записей в таблице ProducedProducts, для которых " +
                        "количество продукции меньше заданного", 1, "center");
                    htmlBuilder.OpenForm("post", "/search_form1");
                    htmlBuilder.OpenLabel();
                    htmlBuilder.AddText("Введите количество продукции:  ");
                    htmlBuilder.CreateInput("ProductsQuantity", "number", productsQuantity == null ? "" : productsQuantity);
                    htmlBuilder.CloseLabel();
                    htmlBuilder.CreateInput("SearchButton", "submit", "Поиск");
                    htmlBuilder.CloseForm();
                    htmlBuilder.CreateBr();

                    if (context.Request.Method == "POST")
                    {
                        productsQuantity = context.Request.Form["ProductsQuantity"];
                        context.Response.Cookies.Append("ProductsQuantity", productsQuantity);

                        IEnumerable<ProducedProduct> filteredRecords = records.Where(record => 
                        record.ProductQuantity < int.Parse(productsQuantity));

                        if (filteredRecords.Count() == 0)
                            htmlBuilder.CreateH("Записи, удовлетворяющие условию, не найдены.", 1, "center");
                        else
                        {
                            string[] headers = { "ID", "ID организации", "ID вида продукции", "Количество продукции", 
                                "Количество затраченной теплоэнергии", "Дата" };
                            htmlBuilder.CreateTable(headers, filteredRecords.ToList());
                        }
                    }

                    htmlBuilder.CreateBr();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("Главная", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Страница с формой для поиска записей в таблице ProducedProducts с использованием sessions
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
                    htmlBuilder.CreateTitle("Поиск записей в таблице ProducedProducts");
                    htmlBuilder.CloseHead();
                    htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                    htmlBuilder.OpenBody();
                    htmlBuilder.CreateH("Поиск записей в таблице ProducedProducts, для которых " +
                        "количество продукции меньше заданного", 1, "center");
                    htmlBuilder.OpenForm("post", "/search_form2");
                    htmlBuilder.OpenLabel();
                    htmlBuilder.AddText("Введите количество продукции:  ");
                    htmlBuilder.CreateInput("ProductsQuantity", "number", 
                        producedProduct.ProductQuantity == 0 ? "" : producedProduct.ProductQuantity.ToString());
                    htmlBuilder.CloseLabel();
                    htmlBuilder.CreateInput("SearchButton", "submit", "Поиск");
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
                            htmlBuilder.CreateH("Записи, удовлетворяющие условию, не найдены.");
                        else
                        {
                            string[] headers = { "ID", "ID организации", "ID вида продукции", "Количество продукции", 
                                "Количество затраченной теплоэнергии", "Дата" };
                            htmlBuilder.CreateTable(headers, filteredRecords.ToList());
                        }
                    }

                    htmlBuilder.CreateBr();
                    htmlBuilder.OpenDiv("center");
                    htmlBuilder.CreateA("Главная", "/");
                    htmlBuilder.CloseDiv();
                    htmlBuilder.CloseBody();
                    htmlBuilder.CloseHtml();

                    await context.Response.WriteAsync(htmlBuilder.Code);
                });
            });

            // Главная страница
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
                htmlBuilder.CreateTitle("Главная страница");
                htmlBuilder.CloseHead();
                htmlBuilder.CreateMeta("Content-Type", "text/html", "utf-8");
                htmlBuilder.OpenBody();
                htmlBuilder.CreateH("Меню", 1, "center");
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Информация о клиенте", "/info");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о главных энергетиках", "/chief_power_engineers");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о нормах расхода теплоэнергии", "/heat_energy_consumption_rates");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о руководителях", "/managers");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией об организациях", "/organizations");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о формах собственности", "/ownership_forms");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о произведенной продукции", "/produced_products");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о типах продукции", "/products_types");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией об оказанных услугах", "/provided_services");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Таблица с информацией о типах услуг", "/services_types");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Форма для поиска произведенной продукции с использованием cookies", "/search_form1");
                htmlBuilder.CloseDiv();
                htmlBuilder.CreateBr();
                htmlBuilder.OpenDiv("center");
                htmlBuilder.CreateA("Форма для поиска произведенной продукции с использованием sessions", "/search_form2");
                htmlBuilder.CloseDiv();
                htmlBuilder.CloseBody();
                htmlBuilder.CloseHtml();

                await context.Response.WriteAsync(htmlBuilder.Code);
            });

            app.Run();
        }
    }
}