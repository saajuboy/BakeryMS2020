using System.Net.Http;
using System;
using System.Text;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class ReportRepository : IReportRepository
    {
        private readonly DataContext _context;
        public ReportRepository(DataContext context)
        {
            _context = context;

        }
        public async Task<string> GetItemReportHtmlString()
        {
            var items = await _context.Items.Include(a => a.ItemCategory).Include(b => b.Unit).ToListAsync();
            var html = new StringBuilder();
            html.AppendFormat(@"<!DOCTYPE html>
                        <html class=""text-danger"">
                        <head><meta charset=""utf-8"" /><meta name=""viewport"" content=""width=device-width, initial-scale=1.0, shrink-to-fit=no""/>
                            <title>Report</title>
                            <link rel=""stylesheet"" href=""assets/css/styles.css"" />
                            <link
                            href=""https://fonts.googleapis.com/css?family=Anton""
                            rel=""stylesheet""
                            />
                        </head>

                        <body>
                            <header>
                            <div>
                                <h1>Upland Bake House</h1>
                            </div>
                            <div class=""Logo-Line"">
                                <label >Truly Lankan</label>
                            </div>
                            <hr />
                            <div class=""Report-Info"">
                                
                                <label >Report Time :{0}</label>
                                <!-- <label style=""margin-left: 332px"">Printed:</label> -->
                            </div>
                            <div>
                                <h3 class=""Report-Title"" style=""margin-top: 50px"">
                                Item report : {1}
                                </h3>
                            </div>
                            </header>
                            <main>
                                <div class=""t-container"" >
                                <div >
                                <table class=""table"">
                                    <thead>
                                    <tr class=""t-head"">
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Number</th>
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Name</th>
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Code</th>
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Description</th>
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Category</th>
            <th style=""border: 2px solid black;padding-top: 10px;padding-bottom: 10px;padding-right: 20px;padding-left: 20px;"">Unit<th>
                        </tr>
                    </thead>
                    <tbody>", DateTime.Now.ToString(), DateTime.Today.ToShortDateString());

            var num = 0;
            foreach (var item in items)
            {
                num = num + 1;
                html.AppendFormat(@"
                            <tr>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{0}</td>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{1}</td>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{2}</td>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{3}</td>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{4}</td>
            <td style=""border: 2px solid black;padding-top: 5px;padding-bottom: 5px;padding-right: 10px;padding-left: 10px;"">{5}</td>
                            </tr>
                            ", num,
                            item.Name,
                            item.Code,
                            item.Description == null ? "" : item.Description,
                            item.ItemCategory.Code + item.ItemCategory.Description == null ? "" : ": " + item.ItemCategory.Description,
                            item.Unit.Description == null ? "" : item.Unit.Description);

            }

            html.Append(@"</tbody>
                        </table>
                        </div>
                        </div>
                        </main>
                        </body>
                        </html>");


            return html.ToString();
        }
    }
}