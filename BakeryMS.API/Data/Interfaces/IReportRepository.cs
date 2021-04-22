using System.Threading.Tasks;
using BakeryMS.API.Models;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IReportRepository
    {
        Task<string> GetItemReportHtmlString(int? itemType, string wildCard);
        Task<string> GetSupplierReportHtmlString(string wildCard);
        Task<string> GetCustomerReportHtmlString(string wildCard);
        Task<string> GetBusinessPlaceReportHtmlString(string wildCard);
        Task<string> GetUnitReportHtmlString(string wildCard);
        Task<string> GetItemCategoryReportHtmlString(string wildCard);
        Task<string> GetSalesReportHtmlString(int? range, string date, int? month, int? year, string wildCard);
        Task<string> GetExpensesReportHtmlString(int? range, string date, int? month, int? year, string wildCard);
        Task<string> GetExpenseIncomeReportHtmlString(int? range, string date, int? month, int? year, string wildCard);
        Task<string> GetStockReportHtmlString(int? range, string date, int? month, int? year, string wildCard);
        Task<string> GetIngredientsReportHtmlString(int? itemId, string wildCard);
        Task<Item> GetItem(int id);


    }
}