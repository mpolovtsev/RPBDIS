using System.Text;
using HeatEnergyConsumption.Models;

namespace HeatEnergyConsumption.HTML
{
    public class HTMLBuilder
    {
        StringBuilder code;
        int levelH = 1;

        public HTMLBuilder() => code = new StringBuilder();

        public string Code => code.ToString();

        public void OpenHtml() => code.Append("<html>");

        public void CloseHtml() => code.Append("</html>");

        public void OpenHead() => code.Append("<head>");

        public void CloseHead() => code.Append("</head>");

        public void OpenBody() => code.Append("<body>");

        public void CloseBody() => code.Append("</body>");

        public void CreateTitle(string content) => code.Append($"<title>{content}</title>");

        public void CreateMeta(string httpEquiv, string content, string charset) => 
            code.Append($"<meta http-equiv=\"{httpEquiv}\" content=\"{content}\" charset=\"{charset}\">");

        public void CreateA(string content, string href) => code.Append($"<a href=\"{href}\">{content}</a>");

        public void OpenA(string href) => code.Append($"<a href=\"{href}\">");

        public void CloseA() => code.Append("</a>");

        public void CreateB(string content) => code.Append($"<b>{content}</b>");

        public void OpenB() => code.Append("<b>");

        public void CloseB() => code.Append("</b>");

        public void CreateBr() => code.Append("<br>");

        public void CreateDiv(string content, string align = "left") => 
            code.Append($"<div align = \"{align}\">{content}</div>");

        public void OpenDiv(string align = "left") => code.Append($"<div align = \"{align}\">");

        public void CloseDiv() => code.Append("</div>");

        public void OpenForm(string method, string action) => code.Append($"<form method=\"{method}\" action=\"{action}\">");

        public void CloseForm() => code.Append("</form>");

        public void CreateH(string content, int level = 1, string align = "left")
        {
            levelH = level;
            code.Append($"<h{level} align=\"{align}\">{content}</h{level}>");
        }

        public void OpenH(int level = 1, string align = "left")
        {
            levelH = level;
            code.Append($"<h{level} align=\"{align}\">");
        }

        public void CloseH() => code.Append($"</h{levelH}>");

        public void CreateInput(string name, string type, string value) => code.Append($"<input name=\"{name}\" type=\"{type}\" value=\"{value}\">");

        public void OpenLabel() => code.Append("<label>");
        
        public void CloseLabel() => code.Append("</label>");

        public void CreateP(string content) => code.Append($"<p>{content}</p>");

        public void OpenP() => code.Append("<p>");

        public void CloseP() => code.Append("</p>");

        public void CreateTable<T>(string[] headers, List<T> records, string align = "center", int border = 1)
        {
            OpenTable(align, border);
            OpenTr();

            foreach (string header in headers)
            {
                CreateTh(header);
            }

            CloseTr();

            switch (records)
            {
                case List<ChiefPowerEngineer> chiefPowerEngineers:
                    CreateTableBodyChiefPowerEngineers(chiefPowerEngineers);
                    break;
                case List<HeatEnergyConsumptionRate> heatEnergyConsumptionRates:
                    CreateTableBodyHeatEnergyConsumptionRates(heatEnergyConsumptionRates);
                    break;
                case List<Manager> managers:
                    CreateTableBodyManagers(managers);
                    break;
                case List<Organization> organizations:
                    CreateTableBodyOrganizations(organizations);
                    break;
                case List<OwnershipForm> ownershipForms:
                    CreateTableBodyOwnershipForms(ownershipForms);
                    break;
                case List<ProducedProduct> producedProducts:
                    CreateTableBodyProducedProducts(producedProducts);
                    break;
                case List<ProductsType> productsTypes:
                    CreateTableBodyProductsTypes(productsTypes);
                    break;
                case List<ProvidedService> providedServices:
                    CreateTableBodyProvidedServices(providedServices);
                    break;
                case List<ServicesType> servicesTypes:
                    CreateTableBodyServicesTypes(servicesTypes);
                    break;
            }

            CloseTable();
        }

        public void OpenTable(string align = "center", int border = 1) => 
            code.Append($"<table align=\"{align}\" border=\"{border}\">");

        public void CloseTable() => code.Append("</table>");

        public void CreateTd(string content) => code.Append($"<td>{content}</td>");

        public void OpenTd() => code.Append("<td>");

        public void CloseTd() => code.Append("</td>");

        public void CreateTh(string content) => code.Append($"<th>{content}</th>");

        public void OpenTh() => code.Append("<th>");

        public void CloseTh() => code.Append("</th>");

        public void OpenTr() => code.Append("<tr>");

        public void CloseTr() => code.Append("</tr>");

        public void AddText(string content) => code.Append(content);

        public void AddHtmlCode(string content) => code.Append(content);

        void CreateTableBodyChiefPowerEngineers(List<ChiefPowerEngineer> records)
        {
            foreach (ChiefPowerEngineer record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Name);
                CreateTd(record.Surname);
                CreateTd(record.MiddleName);
                CreateTd(record.PhoneNumber);
                CreateTd(record.OrganizationId.ToString());
                CloseTr();
            }
        }

        void CreateTableBodyHeatEnergyConsumptionRates(List<HeatEnergyConsumptionRate> records)
        {
            foreach (HeatEnergyConsumptionRate record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.OrganizationId.ToString());
                CreateTd(record.ProductTypeId.ToString());
                CreateTd(record.Quantity.ToString());
                CreateTd(record.Date.ToString("dd/mm/yyyy"));
                CloseTr();
            }
        }

        void CreateTableBodyManagers(List<Manager> records)
        {
            foreach (Manager record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Name);
                CreateTd(record.Surname);
                CreateTd(record.MiddleName);
                CreateTd(record.PhoneNumber);
                CloseTr();
            }
        }

        void CreateTableBodyOrganizations(List<Organization> records)
        {
            foreach (Organization record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Name);
                CreateTd(record.OwnershipFormId.ToString());
                CreateTd(record.Address);
                CreateTd(record.ManagerId.ToString());
                CloseTr();
            }
        }

        void CreateTableBodyOwnershipForms(List<OwnershipForm> records)
        {
            foreach (OwnershipForm record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Name);
                CloseTr();
            }
        }

        void CreateTableBodyProducedProducts(List<ProducedProduct> records)
        {
            foreach (ProducedProduct record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.OrganizationId.ToString());
                CreateTd(record.ProductTypeId.ToString());
                CreateTd(record.ProductQuantity.ToString());
                CreateTd(record.HeatEnergyQuantity.ToString());
                CreateTd(record.Date.ToString("dd/mm/yyyy"));
                CloseTr();
            }
        }

        void CreateTableBodyProductsTypes(List<ProductsType> records)
        {
            foreach (ProductsType record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Code);
                CreateTd(record.Name);
                CreateTd(record.Unit);
                CloseTr();
            }
        }

        void CreateTableBodyProvidedServices(List<ProvidedService> records)
        {
            foreach (ProvidedService record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.OrganizationId.ToString());
                CreateTd(record.ServiceTypeId.ToString());
                CreateTd(record.Quantity.ToString());
                CreateTd(record.Date.ToString("dd/mm/yyyy"));
                CloseTr();
            }
        }

        void CreateTableBodyServicesTypes(List<ServicesType> records)
        {
            foreach (ServicesType record in records)
            {
                OpenTr();
                CreateTd(record.Id.ToString());
                CreateTd(record.Code);
                CreateTd(record.Name);
                CreateTd(record.Unit);
                CloseTr();
            }
        }

        public void CreatePageWithTable<T>(string title, string[] headers, IEnumerable<T> records)
        {
            OpenHtml();
            OpenHead();
            CreateTitle(title);
            CloseHead();
            CreateMeta("Content-Type", "text/html", "utf-8");
            OpenBody();
            CreateH(title, 1, "center");
            CreateTable(headers, records.ToList());
            CreateBr();
            OpenDiv("center");
            CreateA("Главная", "/");
            CloseDiv();
            CloseBody();
            CloseHtml();
        }
    }
}