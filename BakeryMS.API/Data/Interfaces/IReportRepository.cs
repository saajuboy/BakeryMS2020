using System.Threading.Tasks;

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

    }
}