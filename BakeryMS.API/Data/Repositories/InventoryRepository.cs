using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Common.Params;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Inventory;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly DataContext _context;
        public InventoryRepository(DataContext context)
        {
            _context = context;

        }

        public void Add<T>(T entity) where T : class
        {
            _context.AddRange(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PurchaseOrderHeader> GetPurchaseOrder(int id, POFilterParams filterParams)
        {
            var purchaseOrders = _context.PurchaseOrderHeaders.AsQueryable();

            switch (filterParams.isFromOutlet)
            {
                case 0:
                    purchaseOrders = purchaseOrders.Where(a => a.isFromOutlet == true);
                    break;
                case 1:
                    purchaseOrders = purchaseOrders.Where(a => a.isFromOutlet == false);
                    break;
                default:
                    break;
            }

            if (!filterParams.containNotActive)
            {
                purchaseOrders = purchaseOrders.Where(a => a.Status == true);
            }


            var purchaseOrder = await purchaseOrders
           .Include(p => p.PurchaseOrderDetail).ThenInclude(pd => pd.Item)
           .FirstOrDefaultAsync(a => a.Id == id);

            return purchaseOrder;

        }

        public async Task<IEnumerable<PurchaseOrderHeader>> GetPurchaseOrders(POFilterParams filterParams)
        {
            var purchaseOrders = _context.PurchaseOrderHeaders.AsQueryable();

            switch (filterParams.isFromOutlet)
            {
                case 0:
                    purchaseOrders = purchaseOrders.Where(a => a.isFromOutlet == true);
                    break;
                case 1:
                    purchaseOrders = purchaseOrders.Where(a => a.isFromOutlet == false);
                    break;
                default:
                    break;
            }

            if (!filterParams.containNotActive)
            {
                purchaseOrders = purchaseOrders.Where(a => a.Status == true);
            }
            return await purchaseOrders.ToListAsync();
        }

        public async Task CreatePurchaseOrder(PurchaseOrderHeader purchaseOrderHeader)
        {
            var supID = (int)purchaseOrderHeader.SupplierId;
            purchaseOrderHeader.Supplier = await GetSupplier(supID);

            var maxId = _context.PurchaseOrderHeaders.Max(model => model.PONumber);
            purchaseOrderHeader.PONumber = maxId + 1;
            purchaseOrderHeader.Status = true;

            foreach (var pod in purchaseOrderHeader.PurchaseOrderDetail)
            {
                pod.Item = await _context.Items.FirstOrDefaultAsync(a => a.Id == pod.Item.Id);
            }

            Add(purchaseOrderHeader);
            // await SaveAll();

        }

        public async Task<Supplier> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.FirstOrDefaultAsync(a => a.Id == id);

            return supplier;
        }


    }
}