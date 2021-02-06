using System.Net.Mime;
using System;
using System.IO;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BakeryMS.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class ReportsController : ControllerBase
    {
        private readonly IConverter _converter;
        private readonly IReportRepository _reportRepository;
        public ReportsController(IConverter converter, IReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
            _converter = converter;

        }

        [HttpGet]
        public async Task<IActionResult> GetPdf()
        {
            var globalSetings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 5 },
                DocumentTitle = "report123",
                Out=@"E:\1bit Project 2020\Project\Bakery management System\BakeryMS.API\report.pdf"
            };

            var ObjectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = await _reportRepository.GetItemReportHtmlString(),
                // Page = "https://github.com/rdvojmoc/DinkToPdf/issues/51",
                WebSettings = {DefaultEncoding = "utf-8",
                UserStyleSheet=Path.Combine(Directory.GetCurrentDirectory(),"assets","css","styles.css")},
                HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [Page] of [toPage]", Line = true },
                FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }

            };

            var pdf = new HtmlToPdfDocument
            {
                GlobalSettings = globalSetings,
                Objects = { ObjectSettings }
            };

            var file = _converter.Convert(pdf);

            return File(file, "application/pdf");
        }
    }
}