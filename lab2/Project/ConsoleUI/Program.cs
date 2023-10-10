using ConsoleUI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Collections;

namespace ConsoleUI
{
    class Program
    {
        public static void Main()
        {
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile(@"D:\Development of database applications for information systems\lab2\Project\ConsoleUI\ConfigurationFiles\appsettings.json");
            var config = builder.Build();

            string? connectionString = config.GetConnectionString("ConnectionString");

            if (connectionString is null)
            {
                Console.WriteLine("База данных не найдена.");
                return;
            }

            using (HeatEnergyConsumptionContext dbContext = new HeatEnergyConsumptionContext(connectionString))
            {
                while (true)
                {
                    Console.WriteLine("0. Выход.");
                    Console.WriteLine("1. Выборка всех данных из таблицы, стоящей в схеме базы данных на стороне отношения 'один'.");
                    Console.WriteLine("2. Выборка данных из таблицы, стоящей в схеме базы данных на стороне отношения 'один', " +
                        "отфильтрованных по определенному условию, налагающему ограничения на одно или несколько полей.");
                    Console.WriteLine("3. Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового " +
                        "результата (min, max, avg, сount или др.) по выбранному полю из таблицы, стоящей в схеме базы данных на стороне " +
                        "отношения 'многие'.");
                    Console.WriteLine("4. Выборка данных из двух полей двух таблиц, связанных между собой отношением 'один-ко-многим'.");
                    Console.WriteLine("5. Выборка данных из двух таблиц, связанных между собой отношением 'один-ко-многим' и отфильтрованным " +
                        "по некоторому условию, налагающему ограничения на значения одного или нескольких полей.");
                    Console.WriteLine("6. Вставка данных в таблицу, стоящую на стороне отношения 'один'.");
                    Console.WriteLine("7. Вставка данных в таблицу, стоящую на стороне отношения 'многие'.");
                    Console.WriteLine("8. Удаление данных из таблицы, стоящей на стороне отношения 'один'.");
                    Console.WriteLine("9. Удаление данных из таблицы, стоящей на стороне отношения 'многие'.");
                    Console.WriteLine("10. Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных.");
                    Console.Write("Выберите пункт меню: ");

                    if (!int.TryParse(Console.ReadLine(), out int point))
                    {
                        Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                        continue;
                    }

                    Console.WriteLine();

                    switch (point)
                    {
                        case 0:
                            return;
                        case 1:
                            SelectAllFromManagers(dbContext);
                            break;
                        case 2:
                            Console.Write("Введите код продукта: ");
                            string productCode = Console.ReadLine()!;
                            Console.WriteLine();

                            SelectByCodeFromProductsTypes(dbContext, productCode);
                            break;
                        case 3:
                            SelectAvgQuantityFromProducedProducts(dbContext);
                            break;
                        case 4:
                            SelectWithJoinFromOrganizations(dbContext);
                            break;
                        case 5:
                            Console.Write("Введите код услуги: ");
                            string serviceCode = Console.ReadLine()!;
                            Console.WriteLine();

                            SelectWithJoinFromProvidedServices(dbContext, serviceCode);
                            break;
                        case 6:
                            Console.Write("Введите название формы собственности: ");
                            string ownershipFormName = Console.ReadLine()!;
                            Console.WriteLine();

                            try
                            {
                                InsertIntoOwnershipForms(dbContext, ownershipFormName);
                            }
                            catch (DbUpdateException)
                            {
                                Console.WriteLine("Проверьте корректность вводимых данных.");
                                continue;
                            }
                            break;
                        case 7:
                            Console.Write("Введите название организации: ");
                            string organizationName = Console.ReadLine()!;
                            Console.Write("Введите ID формы собственности: ");
                            
                            if (!int.TryParse(Console.ReadLine(), out int ownershipFormId))
                            {
                                Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                                continue;
                            }

                            Console.Write("Введите адрес организации: ");
                            string organizationAddress = Console.ReadLine()!;
                            Console.Write("Введите ID директора организации: ");

                            if (!int.TryParse(Console.ReadLine(), out int managerId))
                            {
                                Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                                continue;
                            }

                            Console.WriteLine();

                            try
                            {
                                InsertIntoOrganizations(dbContext, organizationName, ownershipFormId, organizationAddress, managerId);
                            }
                            catch (DbUpdateException)
                            {
                                Console.WriteLine("Проверьте корректность вводимых данных.");
                                continue;
                            }
                            break;
                        case 8:
                            Console.Write("Введите ID формы собственности: ");

                            if (!int.TryParse(Console.ReadLine(), out ownershipFormId))
                            {
                                Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                                continue;
                            }

                            DeleteFromOwnershipForms(dbContext, ownershipFormId);
                            break;
                        case 9:
                            Console.Write("Введите ID организации: ");

                            if (!int.TryParse(Console.ReadLine(), out int organizationId))
                            {
                                Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                                continue;
                            }

                            DeleteFromOrganizations(dbContext, organizationId);
                            break;
                        case 10:
                            Console.Write("Введите ID формы собственности: ");

                            if (!int.TryParse(Console.ReadLine(), out ownershipFormId))
                            {
                                Console.WriteLine("\nНеобходимо ввести значение целочисленного типа. Попробуйте ещё раз.\n");
                                continue;
                            }

                            Console.Write("Введите название формы собственности: ");
                            ownershipFormName = Console.ReadLine()!;

                            UpdateInOwnershipForms(dbContext, ownershipFormId, ownershipFormName);
                            break;
                        default:
                            Console.WriteLine("Необходимо ввести значение, соответствующее пункту меню. Попробуйте ещё раз.");
                            break;
                    }
                }
            }
        }

        static void Print(IEnumerable items)
        {
            foreach (var item in items)
                Console.WriteLine($"{item}\n");           
        }

        // Выборка всех данных из таблицы, стоящей в схеме базы данных нас стороне отношения 'один'
        static void SelectAllFromManagers(HeatEnergyConsumptionContext dbContext)
        {
            var managers = dbContext.Managers;

            if (managers.Count() == 0)
            {
                Console.WriteLine("Записей не найдено.\n");
                return;
            }

            Print(managers);
        }

        // Выборка данных из таблицы, стоящей в схеме базы данных на стороне отношения 'один',
        // отфильтрованных по определенному условию, налагающему ограничения на одно или несколько полей
        static void SelectByCodeFromProductsTypes(HeatEnergyConsumptionContext dbContext, string code)
        {
            var productsTypes = from productsType in dbContext.ProductsTypes
                                where productsType.Code == code
                                select productsType;

            if (productsTypes.Count() == 0)
            {
                Console.WriteLine("Записей не найдено.\n");
                return;
            }

            Print(productsTypes);
        }

        // Выборка данных, сгруппированных по любому из полей данных с выводом какого-либо итогового
        // результата (min, max, avg, сount или др.) по выбранному полю из таблицы, стоящей в схеме
        // базы данных нас стороне отношения 'многие'
        static void SelectAvgQuantityFromProducedProducts(HeatEnergyConsumptionContext dbContext)
        {
            var producedProducts = from product in dbContext.ProducedProducts
                                   join organization in dbContext.Organizations
                                        on product.OrganizationId equals organization.Id
                                   join productsType in dbContext.ProductsTypes
                                        on product.ProductTypeId equals productsType.Id
                                   group product by new
                                   {
                                       OrganizationId = organization.Id,
                                       ProductsTypeId = productsType.Id,
                                       Date = product.Date.Year
                                   } into products
                                   orderby products.Key.OrganizationId
                                   select new
                                   {
                                       OrganizationName = dbContext.Organizations
                                                        .Where(organization => organization.Id == products.Key.OrganizationId)
                                                        .Select(organization => organization.Name).First(),
                                       ProductsTypeName = dbContext.ProductsTypes
                                                        .Where(productType => productType.Id == products.Key.ProductsTypeId)
                                                        .Select(productType => productType.Name).First(),
                                       Date = products.Key.Date,
                                       AvgQuantity = products.ToList().Average(product => product.ProductQuantity)
                                   };

            if (producedProducts.Count() == 0)
            {
                Console.WriteLine("Записей не найдено.\n");
                return;
            }

            foreach (var product in producedProducts)
            {
                Console.WriteLine($"Organization name: {product.OrganizationName},\nProduct type: {product.ProductsTypeName},\n" +
                    $"Date: {product.Date},\nAverage quantity: {product.AvgQuantity}.\n");
            }
        }

        // Выборка данных из двух полей двух таблиц, связанных между собой отношением 'один-ко-многим'
        static void SelectWithJoinFromOrganizations(HeatEnergyConsumptionContext dbContext)
        {
            var organizations = from organization in dbContext.Organizations
                                join ownershipForm in dbContext.OwnershipForms on
                                    organization.OwnershipFormId equals ownershipForm.Id
                                select new
                                {
                                    OrganizationName = organization.Name,
                                    OwnershipFormName = ownershipForm.Name
                                };

            if (organizations.Count() == 0)
            {
                Console.WriteLine("Записей не найдено.\n");
                return;
            }

            foreach (var organization in organizations)
                Console.WriteLine($"Organization name: {organization.OrganizationName},\n" +
                    $"Ownership form: {organization.OwnershipFormName}.\n");
        }

        // Выборка данных из двух таблиц, связанных между собой отношением 'один-ко-многим' и
        // отфильтрованным по некоторому условию, налагающему ограничения на значения одного или
        // нескольких полей
        static void SelectWithJoinFromProvidedServices(HeatEnergyConsumptionContext dbContext, string code)
        {
            var providedServices = from providedService in dbContext.ProvidedServices
                                   join servicesType in dbContext.ServicesTypes on
                                        providedService.ServiceTypeId equals servicesType.Id
                                   where servicesType.Code == code
                                   select new
                                   {
                                       Name = servicesType.Name,
                                       Code = servicesType.Code,
                                       Quantity = providedService.Quantity,
                                       Date = providedService.Date
                                   };

            if (providedServices.Count() == 0)
            {
                Console.WriteLine("Записей не найдено.\n");
                return;
            }

            foreach (var providedService in providedServices)
                Console.WriteLine($"Service type name: {providedService.Name},\n" +
                    $"Service type code: {providedService.Code},\n" +
                    $"Services quantity: {providedService.Quantity},\n" +
                    $"Date: {providedService.Date.Day}/{providedService.Date.Month}/{providedService.Date.Year}.\n");
        }

        // Вставка данных в таблицу, стоящую на стороне отношения 'один'
        static void InsertIntoOwnershipForms(HeatEnergyConsumptionContext dbContext, string name)
        {
            OwnershipForm ownershipForm = new OwnershipForm()
            {
                Name = name
            };

            dbContext.OwnershipForms.Add(ownershipForm);
            dbContext.SaveChanges();

            Console.WriteLine($"\nДобавлен элемент OwnershipForm.\n\nId: {ownershipForm.Id},\nName: {ownershipForm.Name}.\n");
        }

        // Вставка данных в таблицу, стоящую на стороне отношения 'многие'
        static void InsertIntoOrganizations(HeatEnergyConsumptionContext dbContext, string name, int ownershipFormId,
            string address, int managerId)
        {
            Organization organization = new Organization()
            {
                Name = name,
                OwnershipFormId = ownershipFormId,
                Address = address,
                ManagerId = managerId
            };

            dbContext.Organizations.Add(organization);
            dbContext.SaveChanges();

            Console.WriteLine($"Добавлен элемент Organization.\n\nId: {organization.Id},\nName: {organization.Name},\n" +
                $"OwnershipFormId: {organization.OwnershipFormId},\nAddress: {organization.Address},\n" +
                $"ManagerId: {organization.ManagerId}.\n");
        }

        // Удаление данных из таблицы, стоящей на стороне отношения 'один'
        static void DeleteFromOwnershipForms(HeatEnergyConsumptionContext dbContext, int id)
        {
            var ownershipForms = from ownershipForm in dbContext.OwnershipForms
                                where ownershipForm.Id == id
                                select ownershipForm;

            if (ownershipForms.Count() == 0)
            {
                Console.WriteLine("\nЗаписей не найдено.\n");
                return;
            }

            int count = ownershipForms.Count();

            dbContext.RemoveRange(ownershipForms);
            dbContext.SaveChanges();

            Console.WriteLine($"\nЗаписи удалены ({count}).\n");
        }

        // Удаление данных из таблицы, стоящей на стороне отношения 'многие'
        static void DeleteFromOrganizations(HeatEnergyConsumptionContext dbContext, int id)
        {
            var organizations = from organization in dbContext.Organizations
                                where organization.Id == id
                                select organization;

            if (organizations.Count() == 0)
            {
                Console.WriteLine("\nЗаписей не найдено.\n");
                return;
            }

            int count = organizations.Count();

            dbContext.RemoveRange(organizations);
            dbContext.SaveChanges();

            Console.WriteLine($"\nЗаписи удалены ({count}).\n");
        }

        // Обновление удовлетворяющих определенному условию записей в любой из таблиц базы данных 
        static void UpdateInOwnershipForms(HeatEnergyConsumptionContext dbContext, int id, string name)
        {
            var ownershipForm = (from form in dbContext.OwnershipForms
                                where form.Id == id
                                select form).First();

            if (ownershipForm is null)
            {
                Console.WriteLine("\nЗаписей не найдено.\n");
                return;
            }

            ownershipForm.Name = name;
            dbContext.SaveChanges();

            Console.WriteLine($"\nЗапись обновлена.\n\nId = {ownershipForm.Id},\nName = {ownershipForm.Name}.\n");
        }
    }
}