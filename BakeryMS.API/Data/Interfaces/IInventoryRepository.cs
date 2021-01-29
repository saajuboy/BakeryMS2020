using System.Collections.Generic;
using System.Threading.Tasks;
using BakeryMS.API.Common.Params;
using BakeryMS.API.Models;
using BakeryMS.API.Models.Inventory;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IInventoryRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<bool> SaveAll();
        Task<PurchaseOrderHeader> GetPurchaseOrder(int id, POFilterParams filterParams);
        Task<IEnumerable<PurchaseOrderHeader>> GetPurchaseOrders(POFilterParams filterParams);
        Task<Supplier> GetSupplier(int id);
        Task<IEnumerable<Supplier>> GetSuppliers();
        Task CreatePurchaseOrder(PurchaseOrderHeader purchaseOrderHeader);
        Task<IEnumerable<Item>> GetItems();
         Task<Item> GetItem(int id);
        
    }
}