using System.Drawing;
using System.Dynamic;
using System.Net.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using BakeryMS.API.Models;
using System.Collections.Generic;
using BakeryMS.API.Models.POS;
using System.Globalization;
using BakeryMS.API.Common.DTOs.Inventory;

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

            var html = getHtml("Item Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

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

            var html = getHtml("Suppliers Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

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

            var html = getHtml("Customers Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

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

            var html = getHtml("Business Places Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

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

            var html = getHtml("Units Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

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

            var html = getHtml("Item Categories Report", getTableHtml(tableHeaderString, tableBodystring), DateTime.Today.ToShortDateString());

            return html;
        }

        public async Task<string> GetSalesReportHtmlString(int? range, string date, int? month, int? year, string wildCard)
        {
            if (range != null)
            {
                if (range == 0)
                {
                    var salesQuery = _context.SalesDetails.AsQueryable();

                    DateTime reqDate;

                    if (date == "" || date == null)
                        date = DateTime.Today.ToString();

                    if (DateTime.TryParse(date, out reqDate))
                    {
                        salesQuery = salesQuery.Where(a => a.SalesHeader.Date.Date == reqDate.Date);
                    }
                    else
                    {
                        salesQuery = salesQuery.Where(a => a.SalesHeader.Date.Date == DateTime.Today.Date);
                    }

                    if (wildCard != "" && wildCard != null)
                        salesQuery = salesQuery.Where(a => a.SalesHeader.CustomerName.Contains(wildCard));

                    var sales = await salesQuery.ToListAsync();

                    List<SalesDetail> salesDetails = new List<SalesDetail>();

                    foreach (var item in sales)
                    {
                        if (!salesDetails.Any(a => a.ItemId == item.ItemId && a.Type == item.Type))
                        {
                            salesDetails.Add(new SalesDetail
                            {
                                Type = item.Type,
                                ItemId = item.ItemId,
                                Price = item.Price,
                                LineTotal = sales.Where(a => a.ItemId == item.ItemId && a.Type == item.Type).Sum(a => a.LineTotal),
                                Quantity = sales.Where(a => a.ItemId == item.ItemId && a.Type == item.Type).Sum(a => a.Quantity)
                            });
                        }
                    }

                    List<string> columns = new List<string>();
                    columns.Add("No.");
                    columns.Add("Item Code");
                    columns.Add("Description");
                    columns.Add("Quantity");
                    columns.Add("Unit Price");
                    columns.Add("Total");

                    string tableHeaderString = getTableHeader(columns);

                    var tableBody = new StringBuilder();

                    var prodItems = await _context.ProductionItems.Include(a => a.Item).ToListAsync();
                    var compItems = await _context.CompanyItems.Include(a => a.Item).ToListAsync();

                    var num = 0;
                    foreach (var item in salesDetails)
                    {
                        num = num + 1;
                        tableBody.AppendFormat(@"<tr>");

                        List<string> values = new List<string>();
                        values.Add(num.ToString());

                        var thisItem = item.Type == 0 ? prodItems.FirstOrDefault(a => a.Id == item.ItemId).Item : compItems.FirstOrDefault(a => a.Id == item.ItemId).Item;
                        values.Add(thisItem.Code);
                        values.Add(thisItem.Name);

                        values.Add(item.Quantity.ToString());
                        values.Add(item.Price.ToString());
                        values.Add(item.LineTotal.ToString());

                        tableBody.AppendFormat(getTableBody(values));
                        tableBody.AppendFormat(@"</tr>");
                    }

                    string tableBodystring = tableBody.ToString();

                    List<string> keyValue = new List<string>();
                    keyValue.Add(GetSummaryKeyValueString("Total Sales For the Day", "Rs " + salesDetails.Sum(a => a.LineTotal).ToString()));

                    var summaryString = GetSummaryHtml(keyValue);

                    var bodyHtml = getTableHtml(tableHeaderString, tableBodystring) + summaryString;

                    var html = getHtml("Daily Sales Report", bodyHtml, reqDate.ToShortDateString());

                    return html;
                }
                else
                {
                    if (!month.HasValue || month.Value < 1 || month.Value > 12)
                        month = DateTime.Today.Month;
                    if (!year.HasValue || year.Value < 2000 || year.Value > 2100)
                        year = DateTime.Today.Year;

                    var salesQuery = _context.SalesHeaders.AsQueryable();

                    DateTime reqDate = DateTime.Today;

                    if (range == 1)
                    {
                        salesQuery = salesQuery.Where(a => a.Date.Month == month.Value && a.Date.Year == year.Value);
                    }
                    else
                    {
                        salesQuery = salesQuery.Where(a => a.Date.Year == year);
                    }

                    if (wildCard != "" && wildCard != null)
                        salesQuery = salesQuery.Where(a => a.CustomerName.Contains(wildCard));

                    var sales = await salesQuery.ToListAsync();
                    List<SalesHeader> salesHeaders = new List<SalesHeader>();

                    foreach (var item in sales)
                    {
                        if (item.CustomerName.Contains("Cash"))
                        {
                            if (!salesHeaders.Any(a => a.CustomerName.Contains("Cash")))
                            {
                                salesHeaders.Add(new SalesHeader
                                {
                                    CustomerName = "Cash Payees",
                                    Total = sales.Where(a => a.CustomerName.Contains("Cash")).Sum(a => a.Total)
                                });
                            }
                        }
                        else
                        {
                            if (!salesHeaders.Any(a => item.CustomerId.HasValue && a.CustomerId.Value == item.CustomerId.Value))
                            {
                                salesHeaders.Add(new SalesHeader
                                {
                                    CustomerName = item.CustomerName,
                                    Total = sales.Where(a => a.CustomerId == item.CustomerId).Sum(a => a.Total),

                                });
                            }
                            if (!item.CustomerId.HasValue)
                            {
                                salesHeaders.Add(new SalesHeader
                                {
                                    CustomerName = item.CustomerName,
                                    Total = item.Total,

                                });
                            }
                        }

                    }

                    List<string> columns = new List<string>();
                    columns.Add("No.");
                    columns.Add("Customer");
                    columns.Add("Amount");

                    string tableHeaderString = getTableHeader(columns);

                    var tableBody = new StringBuilder();

                    var num = 0;
                    foreach (var item in salesHeaders)
                    {
                        num = num + 1;
                        tableBody.AppendFormat(@"<tr>");

                        List<string> values = new List<string>();
                        values.Add(num.ToString());
                        values.Add(item.CustomerName);
                        values.Add(item.Total.ToString());

                        tableBody.AppendFormat(getTableBody(values));
                        tableBody.AppendFormat(@"</tr>");
                    }

                    string tableBodystring = tableBody.ToString();
                    string summaryText = "";
                    if (range == 1)
                        summaryText = "Month of " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value) + " " + year;
                    if (range == 2)
                        summaryText = "Year of " + year;

                    List<string> keyValue = new List<string>();
                    keyValue.Add(GetSummaryKeyValueString("Total Sales For the " + summaryText, "Rs " + salesHeaders.Sum(a => a.Total).ToString()));

                    var summaryString = GetSummaryHtml(keyValue);

                    var bodyHtml = getTableHtml(tableHeaderString, tableBodystring) + summaryString;

                    var html = getHtml(range == 1 ? "Monthly" : "Yearly" + " Sales Report", bodyHtml, summaryText);

                    return html;

                }

            }

            return "";

        }

        public async Task<string> GetExpensesReportHtmlString(int? range, string date, int? month, int? year, string wildCard)
        {
            if (range != null)
            {

                var transQuery = _context.Transactions.Where(a => a.Credit > 0).AsQueryable();

                DateTime reqDate;

                if (date == "" || date == null)
                    date = DateTime.Today.ToString();
                if (!month.HasValue || month.Value < 1 || month.Value > 12)
                    month = DateTime.Today.Month;
                if (!year.HasValue || year.Value < 2000 || year.Value > 2100)
                    year = DateTime.Today.Year;

                if (range == 0)
                {
                    if (DateTime.TryParse(date, out reqDate))
                    {
                        transQuery = transQuery.Where(a => a.Date.Date == reqDate.Date);
                    }
                    else
                    {
                        transQuery = transQuery.Where(a => a.Date.Date == DateTime.Today.Date);
                    }
                }
                else if (range == 1)
                {
                    transQuery = transQuery.Where(a => a.Date.Month == month.Value && a.Date.Year == year.Value);
                }
                else if (range == 2)
                {
                    transQuery = transQuery.Where(a => a.Date.Year == year);
                }

                if (wildCard != "" && wildCard != null)
                    transQuery = transQuery.Where(a => a.Reference.Contains(wildCard) || a.Description.Contains(wildCard));

                var transactions = await transQuery.ToListAsync();

                List<Transaction> salesDetails = new List<Transaction>();

                if (range == 0)
                {
                    salesDetails = transactions;
                }
                else
                {
                    foreach (var item in transactions)
                    {
                        if (!salesDetails.Any(a => a.Reference.Contains(item.Reference)))
                        {
                            salesDetails.Add(new Transaction
                            {
                                Description = item.Description + "(Grouped Description Date Not Valid)",
                                Reference = item.Reference,
                                Credit = transactions.Where(a => a.Reference.Contains(item.Reference)).Sum(a => a.Credit)
                            });
                        }
                    }
                }


                List<string> columns = new List<string>();
                columns.Add("No.");
                columns.Add("Description");
                columns.Add("Reference");
                if (range == 0)
                    columns.Add("Time");
                columns.Add("Amount");

                string tableHeaderString = getTableHeader(columns);

                var tableBody = new StringBuilder();

                var num = 0;
                foreach (var item in salesDetails)
                {
                    num = num + 1;
                    tableBody.AppendFormat(@"<tr>");

                    List<string> values = new List<string>();
                    values.Add(num.ToString());
                    values.Add(item.Description);
                    values.Add(item.Reference);
                    if (range == 0)
                        values.Add(item.Time.Value.ToString(@"hh\:mm"));
                    values.Add(item.Credit.ToString());

                    tableBody.AppendFormat(getTableBody(values));
                    tableBody.AppendFormat(@"</tr>");
                }

                string tableBodystring = tableBody.ToString();

                string summaryText = "";
                if (range == 0)
                    summaryText = "Day " + date;
                if (range == 1)
                    summaryText = "Month of " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value) + " " + year;
                if (range == 2)
                    summaryText = "Year of " + year;

                List<string> keyValue = new List<string>();
                keyValue.Add(GetSummaryKeyValueString("Total Expense For the " + summaryText, "Rs " + salesDetails.Sum(a => a.Credit).ToString()));

                var summaryString = GetSummaryHtml(keyValue);

                var bodyHtml = getTableHtml(tableHeaderString, tableBodystring) + summaryString;

                var html = getHtml((range == 0 ? "Daily" : range == 1 ? "Monthly" : "Yearly") + " Expense Report", bodyHtml, summaryText);

                return html;
            }

            return "";
        }

        public async Task<string> GetExpenseIncomeReportHtmlString(int? range, string date, int? month, int? year, string wildCard)
        {
            if (range != null)
            {

                var transQuery = _context.Transactions.AsQueryable();

                DateTime reqDate;

                if (date == "" || date == null)
                    date = DateTime.Today.ToString();
                if (!month.HasValue || month.Value < 1 || month.Value > 12)
                    month = DateTime.Today.Month;
                if (!year.HasValue || year.Value < 2000 || year.Value > 2100)
                    year = DateTime.Today.Year;

                if (range == 0)
                {
                    if (DateTime.TryParse(date, out reqDate))
                    {
                        transQuery = transQuery.Where(a => a.Date.Date == reqDate.Date);
                    }
                    else
                    {
                        transQuery = transQuery.Where(a => a.Date.Date == DateTime.Today.Date);
                    }
                }
                else if (range == 1)
                {
                    transQuery = transQuery.Where(a => a.Date.Month == month.Value && a.Date.Year == year.Value);
                }
                else if (range == 2)
                {
                    transQuery = transQuery.Where(a => a.Date.Year == year);
                }

                if (wildCard != "" && wildCard != null)
                    transQuery = transQuery.Where(a => a.Reference.Contains(wildCard) || a.Description.Contains(wildCard));

                var transactions = await transQuery.ToListAsync();

                List<Transaction> TransactionToReport = new List<Transaction>();

                if (range == 0)
                {
                    TransactionToReport = transactions;
                }
                else
                {
                    foreach (var item in transactions)
                    {
                        if (!TransactionToReport.Any(a => a.Reference.Contains(item.Reference)))
                        {
                            TransactionToReport.Add(new Transaction
                            {
                                Description = item.Description + "(Grouped Description Date Not Valid)",
                                Reference = item.Reference,
                                Credit = transactions.Where(a => a.Reference.Contains(item.Reference)).Sum(a => a.Credit),
                                Debit = transactions.Where(a => a.Reference.Contains(item.Reference)).Sum(a => a.Debit)

                            });
                        }
                    }
                }


                List<string> columns = new List<string>();
                columns.Add("No.");
                columns.Add("Description");
                columns.Add("Reference");
                if (range == 0)
                    columns.Add("Time");
                columns.Add("Debit");
                columns.Add("Credit");

                string tableHeaderString = getTableHeader(columns);

                var tableBody = new StringBuilder();

                var num = 0;
                foreach (var item in TransactionToReport)
                {
                    num = num + 1;
                    tableBody.AppendFormat(@"<tr>");

                    List<string> values = new List<string>();
                    values.Add(num.ToString());
                    values.Add(item.Description);
                    values.Add(item.Reference);
                    if (range == 0)
                        values.Add(item.Time.Value.ToString(@"hh\:mm"));
                    values.Add(item.Debit.ToString());
                    values.Add(item.Credit.ToString());

                    tableBody.AppendFormat(getTableBody(values));
                    tableBody.AppendFormat(@"</tr>");
                }

                string tableBodystring = tableBody.ToString();

                string summaryText = "";
                if (range == 0)
                    summaryText = "Day " + date;
                if (range == 1)
                    summaryText = "Month of " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value) + " " + year;
                if (range == 2)
                    summaryText = "Year of " + year;

                List<string> keyValue = new List<string>();

                keyValue.Add(GetSummaryKeyValueString("Total Income For the " + summaryText, "Rs " + TransactionToReport.Sum(a => a.Debit).ToString()));
                keyValue.Add(GetSummaryKeyValueString("Total Expense For the " + summaryText, "Rs (" + TransactionToReport.Sum(a => a.Credit).ToString() + ")"));
                keyValue.Add(GetSummaryKeyValueString("Total Gross Profit For the " + summaryText, "Rs " + (TransactionToReport.Sum(a => a.Debit) - TransactionToReport.Sum(a => a.Credit)).ToString()));

                var summaryString = GetSummaryHtml(keyValue);

                var bodyHtml = getTableHtml(tableHeaderString, tableBodystring) + summaryString;

                var html = getHtml((range == 0 ? "Daily" : range == 1 ? "Monthly" : "Yearly") + " Expense/Income Report", bodyHtml, summaryText);

                return html;
            }

            return "";
        }

        public async Task<string> GetStockReportHtmlString(int? range, string date, int? month, int? year, string wildCard)
        {
            if (range != null)
            {

                var prodItemQuery = _context.ProductionItems.Include(a => a.Item).ThenInclude(a => a.Unit).AsQueryable();
                var compItemQuery = _context.CompanyItems.Include(a => a.Item).ThenInclude(a => a.Unit).AsQueryable();
                var rawItemQuery = _context.RawItems.Include(a => a.Item).ThenInclude(a => a.Unit).AsQueryable();

                var grnItemQuery = _context.GRNHeaders.AsQueryable();

                DateTime reqDate;

                if (date == "" || date == null)
                    date = DateTime.Today.ToString();
                if (!month.HasValue || month.Value < 1 || month.Value > 12)
                    month = DateTime.Today.Month;
                if (!year.HasValue || year.Value < 2000 || year.Value > 2100)
                    year = DateTime.Today.Year;

                if (range == 0)
                {
                    if (DateTime.TryParse(date, out reqDate))
                    {
                        prodItemQuery = prodItemQuery.Where(a => a.ManufacturedDate.Value.Date == reqDate.Date);
                        grnItemQuery = grnItemQuery.Where(a => a.ReceivedDate.Date == reqDate.Date);
                    }
                    else
                    {
                        prodItemQuery = prodItemQuery.Where(a => a.ManufacturedDate.Value.Date == DateTime.Today.Date);
                        grnItemQuery = grnItemQuery.Where(a => a.ReceivedDate.Date == DateTime.Today.Date);
                    }
                }
                else if (range == 1)
                {
                    prodItemQuery = prodItemQuery.Where(a => a.ManufacturedDate.Value.Month == month.Value && a.ManufacturedDate.Value.Year == year.Value);
                    grnItemQuery = grnItemQuery.Where(a => a.ReceivedDate.Month == month.Value && a.ReceivedDate.Year == year.Value);

                }
                else if (range == 2)
                {
                    prodItemQuery = prodItemQuery.Where(a => a.ManufacturedDate.Value.Year == year);
                    grnItemQuery = grnItemQuery.Where(a => a.ReceivedDate.Year == year.Value);
                }

                if (wildCard != "" && wildCard != null)
                {
                    prodItemQuery = prodItemQuery.Where(a => a.Item.Description.Contains(wildCard) || a.Item.Name.Contains(wildCard) || a.Item.Code.Contains(wildCard));
                    compItemQuery = compItemQuery.Where(a => a.Item.Description.Contains(wildCard) || a.Item.Name.Contains(wildCard) || a.Item.Code.Contains(wildCard));
                    rawItemQuery = rawItemQuery.Where(a => a.Item.Description.Contains(wildCard) || a.Item.Name.Contains(wildCard) || a.Item.Code.Contains(wildCard));
                }


                var grnItems = await grnItemQuery.ToListAsync();


                List<int> poId = new List<int>();

                foreach (var item in grnItems)
                {
                    poId.Add(item.PurchaseOrderHeaderId);
                }

                var prodItems = await prodItemQuery.ToListAsync();
                var compItems = await compItemQuery.Where(a => poId.Contains(a.BatchNo)).ToListAsync();
                var rawItems = await rawItemQuery.Where(a => poId.Contains(a.BatchNo)).ToListAsync();

                List<AvailableItemsDtoForList> stockToReport = new List<AvailableItemsDtoForList>();

                foreach (var item in prodItems)
                {
                    if (!stockToReport.Any(a => a.Code.Contains(item.Item.Code)))
                    {
                        stockToReport.Add(new AvailableItemsDtoForList
                        {
                            Name = item.Item.Name,
                            Code = item.Item.Code,
                            StockedQuantity = prodItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.StockedQuantity),
                            AvailableQuantity = prodItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.AvailableQuantity),
                            UsedQuantity = prodItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.UsedQuantity),
                            Unit = item.Item.Unit.Description
                        });
                    }
                }
                foreach (var item in compItems)
                {
                    if (!stockToReport.Any(a => a.Code.Contains(item.Item.Code)))
                    {
                        stockToReport.Add(new AvailableItemsDtoForList
                        {
                            Name = item.Item.Name,
                            Code = item.Item.Code,
                            StockedQuantity = compItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.StockedQuantity),
                            AvailableQuantity = compItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.AvailableQuantity),
                            UsedQuantity = compItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.UsedQuantity),
                            Unit = item.Item.Unit.Description
                        });
                    }
                }
                foreach (var item in rawItems)
                {
                    if (!stockToReport.Any(a => a.Code.Contains(item.Item.Code)))
                    {
                        stockToReport.Add(new AvailableItemsDtoForList
                        {
                            Name = item.Item.Name,
                            Code = item.Item.Code,
                            StockedQuantity = rawItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.StockedQuantity),
                            AvailableQuantity = rawItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.AvailableQuantity),
                            UsedQuantity = rawItems.Where(a => a.Item.Code == item.Item.Code).Sum(a => a.UsedQuantity),
                            Unit = item.Item.Unit.Description
                        });
                    }
                }

                List<string> columns = new List<string>();
                columns.Add("No.");
                columns.Add("Item Code");
                columns.Add("Description");
                columns.Add("Stocked Quantity");
                columns.Add("Used Quantity");
                columns.Add("Available Quantity");

                string tableHeaderString = getTableHeader(columns);

                var tableBody = new StringBuilder();

                var num = 0;
                foreach (var item in stockToReport)
                {
                    num = num + 1;
                    tableBody.AppendFormat(@"<tr>");

                    List<string> values = new List<string>();
                    values.Add(num.ToString());
                    values.Add(item.Code);
                    values.Add(item.Name);
                    values.Add(item.StockedQuantity.ToString() + " " + item.Unit + (item.StockedQuantity > 1 ? "s" : ""));
                    values.Add(item.UsedQuantity.ToString() + " " + item.Unit + (item.UsedQuantity > 1 ? "s" : ""));
                    values.Add(item.AvailableQuantity.ToString() + " " + item.Unit + (item.AvailableQuantity > 1 ? "s" : ""));

                    tableBody.AppendFormat(getTableBody(values));
                    tableBody.AppendFormat(@"</tr>");
                }

                string tableBodystring = tableBody.ToString();

                string summaryText = "";
                if (range == 0)
                    summaryText = "Day " + date;
                if (range == 1)
                    summaryText = "Month of " + CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(month.Value) + " " + year;
                if (range == 2)
                    summaryText = "Year of " + year;

                // List<string> keyValue = new List<string>();

                // keyValue.Add(GetSummaryKeyValueString("Total Income For the " + summaryText, "Rs " + stockToReport.Sum(a => a.Debit).ToString()));
                // keyValue.Add(GetSummaryKeyValueString("Total Expense For the " + summaryText, "Rs (" + stockToReport.Sum(a => a.Credit).ToString() + ")"));
                // keyValue.Add(GetSummaryKeyValueString("Total Gross Profit For the " + summaryText, "Rs " + (stockToReport.Sum(a => a.Debit) - stockToReport.Sum(a => a.Credit)).ToString()));

                // var summaryString = GetSummaryHtml(keyValue);

                // var bodyHtml = getTableHtml(tableHeaderString, tableBodystring) + summaryString;
                var bodyHtml = getTableHtml(tableHeaderString, tableBodystring);

                var html = getHtml((range == 0 ? "Daily" : range == 1 ? "Monthly" : "Yearly") + " Stock Report", bodyHtml, summaryText);

                return html;
            }

            return "";
        }


        private string getHtml(string title, string body, string date)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<!DOCTYPE html>
            <html class=""text-danger""><head><meta charset=""utf-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1.0, shrink-to-fit=no""/>
            <title>Report</title><link rel=""stylesheet"" href=""assets/css/styles.css"" /><link href=""https://fonts.googleapis.com/css?family=Anton"" rel=""stylesheet"" />
            </head><body><header><div><h1>Upland Bake House</h1></div><div class=""Logo-Line""><label >Truly Lankan</label></div>
            <hr /><div class=""Report-Info""><label >Report Time :{0}</label></div><div><h3 class=""Report-Title"" style=""margin-top: 50px""> {2} : {1} </h3></div></header><main>"
            , DateTime.Now.ToString(), date, title);

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

        private string GetSummaryHtml(List<string> ListOfKeyValuePairString)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<div class=""right"">");

            foreach (var keyValue in ListOfKeyValuePairString)
            {
                html.AppendFormat(@"<div class=""summary"">");
                html.AppendFormat(keyValue);
                html.AppendFormat(@"</div>");
            }
            html.AppendFormat(@"</div>");

            return html.ToString();

        }

        private string GetSummaryKeyValueString(string key, string value)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<div class=""key"">{0} : </div> <div class=""value"">{1}</div>", key, value);

            return html.ToString();
        }
        private string GetBodyHeaderHtml(List<string> ListOfKeyValuePairString)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<div class=""left"">");

            foreach (var keyValue in ListOfKeyValuePairString)
            {
                html.AppendFormat(@"<div class=""bodyHeader"">");
                html.AppendFormat(keyValue);
                html.AppendFormat(@"</div>");
            }
            html.AppendFormat(@"</div>");

            return html.ToString();

        }

        private string GetbodyKeyValueString(string key, string value)
        {
            var html = new StringBuilder();
            html.AppendFormat(@"<div class=""bodyKey"">{0} : </div> <div class=""bodyValue"">{1}</div>", key, value);

            return html.ToString();
        }


    }
}