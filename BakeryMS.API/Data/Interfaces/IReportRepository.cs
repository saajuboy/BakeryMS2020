using System.Threading.Tasks;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IReportRepository
    {
        Task<string> GetItemReportHtmlString();
          
    }
}