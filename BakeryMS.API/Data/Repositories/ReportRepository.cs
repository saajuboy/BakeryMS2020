using System.Net.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BakeryMS.API.Models;
using System.Collections.Generic;

namespace BakeryMS.API.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _context;
        public ReportRepository(DataContext context)
        {
            _context = context;

        }

        public async Task<string> GetItemReportHtmlString(int? itemType, string wildCard)
        {
            var itemsQuery = _context.Items.Include(a => a.ItemCategory).Include(b => b.Unit).AsQueryable();

            if (itemType != null)
                itemsQuery = itemsQuery.Where(a => a.Type == itemType);

            if (wildCard != "" && wildCard != null)
                itemsQuery = itemsQuery.Where(a => a.Name.Contains(wildCard) || a.Description.Contains(wildCard) || a.ItemCategory.Description.Contains(wildCard));

            var items = await itemsQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Name");
            columns.Add("Code");
            columns.Add("Description");
            columns.Add("Category");
            columns.Add("Unit");

            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();

            var num = 0;
            foreach (var item in items)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Name);
                values.Add(item.Code);
                values.Add(item.Description == null ? "" : item.Description);
                values.Add(item.ItemCategory.Code + item.ItemCategory.Description == null ? "" : ": " + item.ItemCategory.Description);
                values.Add(item.Unit.Description == null ? "" : item.Unit.Description);

                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Item Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        public async Task<string> GetSupplierReportHtmlString(string wildCard)
        {
            var supsQuery = _context.Suppliers.AsQueryable();

            if (wildCard != "" && wildCard != null)
                supsQuery = supsQuery.Where(a => a.Name.Contains(wildCard) || a.Address.Contains(wildCard) || a.Email.Contains(wildCard));

            var sups = await supsQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Name");
            columns.Add("Contact");
            columns.Add("Email");
            columns.Add("Type");
            columns.Add("Address");

            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();

            var num = 0;
            foreach (var item in sups)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Name);
                values.Add(item.ContactNumber);
                values.Add(item.Email);
                values.Add(item.Type == 0 ? "Production" : item.Type == 1 ? "Company" : item.Type == 2 ? "Raw" : "Misc");
                values.Add(item.Address);

                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Suppliers Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        public async Task<string> GetCustomerReportHtmlString(string wildCard)
        {
            var cusQuery = _context.Customers.AsQueryable();

            if (wildCard != "" && wildCard != null)
                cusQuery = cusQuery.Where(a => a.Name.Contains(wildCard) || a.Address.Contains(wildCard) || a.Contact.Contains(wildCard));

            var customers = await cusQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Name");
            columns.Add("Contact");
            columns.Add("Address");
            columns.Add("Debit");
            columns.Add("Credit");
            columns.Add("Type");

            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();

            var num = 0;
            foreach (var item in customers)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Name);
                values.Add(item.Contact);
                values.Add(item.Address);
                values.Add(item.Debit.ToString());
                values.Add(item.Credit.ToString());
                values.Add(item.IsRetail == true ? "Retail" : "Wholesale");

                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Customers Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        public async Task<string> GetBusinessPlaceReportHtmlString(string wildCard)
        {
            var placeQuery = _context.BusinessPlaces.AsQueryable();

            if (wildCard != "" && wildCard != null)
                placeQuery = placeQuery.Where(a => a.Name.Contains(wildCard) || a.Address.Contains(wildCard));

            var places = await placeQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Name");
            columns.Add("Reg. No.");
            columns.Add("Address");

            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();
            var num = 0;
            foreach (var item in places)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Name);
                values.Add(item.RegistrationNumber);
                values.Add(item.Address);

                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Business Places Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        public async Task<string> GetUnitReportHtmlString(string wildCard)
        {
            var unitsQuery = _context.Units.AsQueryable();

            if (wildCard != "" && wildCard != null)
                unitsQuery = unitsQuery.Where(a => a.Description.Contains(wildCard));

            var units = await unitsQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Description");
            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();

            var num = 0;
            foreach (var item in units)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Description);
                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Units Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        public async Task<string> GetItemCategoryReportHtmlString(string wildCard)
        {
            var IcatQuery = _context.ItemCategories.AsQueryable();

            if (wildCard != "" && wildCard != null)
                IcatQuery = IcatQuery.Where(a => a.Code.Contains(wildCard) || a.Description.Contains(wildCard));

            var Icats = await IcatQuery.ToListAsync();

            List<string> columns = new List<string>();
            columns.Add("No.");
            columns.Add("Code");
            columns.Add("Description");

            string tableHeaderString = getTableHeader(columns);

            var tableBody = new StringBuilder();

            var num = 0;
            foreach (var item in Icats)
            {
                num = num + 1;
                tableBody.AppendFormat(@"<tr>");

                List<string> values = new List<string>();
                values.Add(num.ToString());
                values.Add(item.Code);
                values.Add(item.Description);

                tableBody.AppendFormat(getTableBody(values));
                tableBody.AppendFormat(@"</tr>");
            }

            string tableBodystring = tableBody.ToString();

            var html = getHtml("Item Categories Report", getTableHtml(tableHeaderString, tableBodystring));

            return html;
        }

        private string getHtml(string title, string body)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<!DOCTYPE html>
            <html class=""text-danger""><head><meta charset=""utf-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1.0, shrink-to-fit=no""/>
            <title>Report</title><link rel=""stylesheet"" href=""assets/css/styles.css"" /><link href=""https://fonts.googleapis.com/css?family=Anton"" rel=""stylesheet"" />
            </head><body><header><div><h1>Upland Bake House</h1></div><div class=""Logo-Line""><label >Truly Lankan</label></div>
            <hr /><div class=""Report-Info""><label >Report Time :{0}</label></div><div><h3 class=""Report-Title"" style=""margin-top: 50px""> {2} : {1} </h3></div></header><main>"
            , DateTime.Now.ToString(), DateTime.Today.ToShortDateString(), title);

            html.AppendFormat(body);

            html.Append(@"</main></body></html>");

            return html.ToString();
        }
        private string getTableHtml(string headerHtml, string bodyHtml)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<div class=""t-container"" ><div ><table class=""table""><thead><tr class=""t-head"">");

            html.AppendFormat(headerHtml);

            html.AppendFormat(@"</tr></thead><tbody>");

            html.AppendFormat(bodyHtml);

            html.AppendFormat(@"</tbody></table></div></div>");

            return html.ToString();

        }
        private string getTableHeader(List<string> ListOfColums)
        {
            var html = new StringBuilder();
            foreach (var item in ListOfColums)
            {
                html.AppendFormat(
                    @"<th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">{0}</th>",
                    item);
            }

            return html.ToString();
        }

        private string getTableBody(List<string> ListOfValues)
        {
            var html = new StringBuilder();
            foreach (var item in ListOfValues)
            {
                html.AppendFormat(
                    @"<td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{0}</td>",
                    item);
            }

            return html.ToString();
        }
    }
}