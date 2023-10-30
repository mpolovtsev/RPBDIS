using HeatEnergyConsumption.Models;
using System.Text;

namespace HTMLBuilderLibrary
{
    public class HTMLBuilder
    {
        StringBuilder htmlCode;

        int levelH = 1;

        public HTMLBuilder() => htmlCode = new StringBuilder();

        public string HtmlCode => htmlCode.ToString();

        public void OpenHtml() => htmlCode.Append("<html>");

        public void CloseHtml() => htmlCode.Append("</html>");

        public void OpenHead() => htmlCode.Append("<head>");

        public void CloseHead() => htmlCode.Append("</head>");

        public void OpenBody() => htmlCode.Append("<html>");

        public void CloseBody() => htmlCode.Append("</body>");

        public void CreateTitle(string content) => htmlCode.Append($"<title>{content}</title>");

        public void CreateMeta(string httpEquiv, string content, string charset) => htmlCode.Append($"<meta http-equiv='{httpEquiv}' content='{content}' charset='{charset}'/>");

        public void CreateA(string content, string href) => htmlCode.Append($"<a href='{href}'>{content}</a>");

        public void OpenA(string href) => htmlCode.Append($"<a href='{href}'>");
        
        public void CloseA() => htmlCode.Append($"</a>");

        public void CreateB(string content) => htmlCode.Append($"<b>{content}</b>");

        public void OpenB() => htmlCode.Append($"<b>");

        public void CloseB() => htmlCode.Append($"</b>");

        public void CreateBr() => htmlCode.Append($"<br>");

        public void CreateDiv(string content, string align = "left") => htmlCode.Append($"<div align = \"{align}\">{content}</div>");

        public void OpenDiv(string align = "left") => htmlCode.Append($"<div align = \"{align}\">");

        public void CloseDiv() => htmlCode.Append($"</div>"); 

        public void CreateH(string content, int level = 1, string align = "left")
        {
            levelH = level;
            htmlCode.Append($"<h{level} align=\"{align}\">{content}</h{level}>");
        }

        public void OpenH(int level = 1, string align = "left")
        {
            levelH = level;
            htmlCode.Append($"<h{level} align=\"{align}\">");
        }

        public void CloseH() => htmlCode.Append($"</h{levelH}>");

        public void CreateP(string content) => htmlCode.Append($"<p>{content}</p>");

        public void OpenP() => htmlCode.Append($"<p>");

        public void CloseP() => htmlCode.Append($"</p>");

        public void CreateTable<T>(string[] headers, List<T> records)
        {
            OpenTable();
            OpenTr();

            foreach (string header in headers)
            {
                CreateTh(header);
            }

            CloseTr();

            foreach (T record in records)
        }

        public void OpenTable() => htmlCode.Append("<table>");

        public void CloseTable() => htmlCode.Append("</table>");

        public void CreateTd(string content) => htmlCode.Append($"<td>{content}</td>");

        public void OpenTd() => htmlCode.Append($"<td>");

        public void CloseTd() => htmlCode.Append($"</td>");

        public void CreateTh(string content) => htmlCode.Append($"<th>{content}</th>");

        public void OpenTh() => htmlCode.Append($"<th>");

        public void CloseTh() => htmlCode.Append($"</th>");

        public void OpenTr() => htmlCode.Append($"<tr>");

        public void CloseTr() => htmlCode.Append($"</tr>");

        public void AddText(string content) => htmlCode.Append(content);

        public void AddHtmlCode(string code) => htmlCode.Append(code);

        string CreateTableBody(IEnumerable<ChiefPowerEngineer> records)
        {
            foreach (ChiefPowerEngineer record in records)
            {

            }
        }
    }
}