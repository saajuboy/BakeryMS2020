using System.Net.Mime;
using System;
using System.IO;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BakeryMS.API.Common.Helpers;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IConverter _converter;
        private readonly IReportRepository _reportRepository;
        private readonly GlobalSettings _globalSettings;
        private readonly ObjectSettings _objectSettings;
        public ReportsController(IConverter converter, IReportRepository reportRepository)
        {
            var globalSetings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10, Bottom = 10, Right = 8, Left = 8 }
            };

            var ObjectSettings = new ObjectSettings
            {
                PagesCount = true,
                WebSettings = {DefaultEncoding = "utf-8",
                UserStyleSheet=Path.Combine(Directory.GetCurrentDirectory(),"assets","css","styles.css")},
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [Page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }

            };

            _objectSettings = ObjectSettings;
            _globalSettings = globalSetings;
            _reportRepository = reportRepository;
            _converter = converter;

        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetMasterReport(int reportType, int? itemType, string wildCard)
        {
            if (reportType == 0)
            {
                _globalSettings.DocumentTitle = "item report";
                _objectSettings.HtmlContent = await _reportRepository.GetItemReportHtmlString(itemType, wildCard);
            }
            if (reportType == 1)
            {
                _globalSettings.DocumentTitle = "Supplier report";
                _objectSettings.HtmlContent = await _reportRepository.GetSupplierReportHtmlString(wildCard);
            }
            if (reportType == 2)
            {
                _globalSettings.DocumentTitle = "Customer report";
                _objectSettings.HtmlContent = await _reportRepository.GetCustomerReportHtmlString(wildCard);
            }
            if (reportType == 3)
            {
                _globalSettings.DocumentTitle = "Business Place report";
                _objectSettings.HtmlContent = await _reportRepository.GetBusinessPlaceReportHtmlString(wildCard);
            }
            if (reportType == 4)
            {
                _globalSettings.DocumentTitle = "Unit report";
                _objectSettings.HtmlContent = await _reportRepository.GetUnitReportHtmlString(wildCard);
            }
            if (reportType == 5)
            {
                _globalSettings.DocumentTitle = "Item Category report";
                _objectSettings.HtmlContent = await _reportRepository.GetItemCategoryReportHtmlString(wildCard);
            }

            return await PrintDoc();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetSalesReport(int reportType, int? range, string date, int? month, int? year, string wildCard)
        {
            range = range == null ? 0 : range;
            var rangeText = range == 0 ? "Daily" : range == 1 ? "Monthly" : range == 2 ? "Yearly" : "";
            if (reportType == 0)
            {
                _globalSettings.DocumentTitle = "Sales report" + rangeText;
                _objectSettings.HtmlContent = await _reportRepository.GetSalesReportHtmlString(range, date, month, year, wildCard);
            }
            if (reportType == 1)
            {
                _globalSettings.DocumentTitle = "Expense report" + rangeText;
                _objectSettings.HtmlContent = await _reportRepository.GetExpensesReportHtmlString(range, date, month, year, wildCard);
            }
            if (reportType == 2)
            {
                _globalSettings.DocumentTitle = "Expense/Income report" + rangeText;
                _objectSettings.HtmlContent = await _reportRepository.GetExpenseIncomeReportHtmlString(range, date, month, year, wildCard);
            }

            return await PrintDoc();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetInventoryReport(int reportType, int? range, string date, int? month, int? year, string wildCard)
        {
            range = range == null ? 0 : range;
            var rangeText = range == 0 ? "Daily" : range == 1 ? "Monthly" : range == 2 ? "Yearly" : "";
            if (reportType == 0)
            {
                _globalSettings.DocumentTitle = "Stock report" + rangeText;
                _objectSettings.HtmlContent = await _reportRepository.GetStockReportHtmlString(range, date, month, year, wildCard);
            }

            return await PrintDoc();
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> GetManufacturingReport(int reportType, int? itemId, string wildCard)
        {
            itemId = itemId == null ? 0 : itemId;
            var item = await _reportRepository.GetItem(itemId.Value);
            var rangeText = itemId == 0 ? "All Items" : (item == null ? "" : item.Name);
            if (reportType == 0)
            {
                _globalSettings.DocumentTitle = "Ingredients report:" + rangeText;
                _objectSettings.HtmlContent = await _reportRepository.GetIngredientsReportHtmlString(itemId, wildCard);
            }

            return await PrintDoc();
        }

        public async Task<IActionResult> PrintDoc()
        {
            if (_objectSettings.HtmlContent != "" && _objectSettings.HtmlContent != null)
            {
                var pdf = new HtmlToPdfDocument
                {
                    GlobalSettings = _globalSettings,
                    Objects = { _objectSettings }
                };

                var file = await Task.FromResult(_converter.Convert(pdf));

                return File(file, "application/pdf");
            }

            return BadRequest(new ErrorModel(1, 400, "Report Not Available"));
        }
    }
}