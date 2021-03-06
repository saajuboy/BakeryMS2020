using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Common.Params;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models;
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
        public void Update<T>(T entity) where T : class
        {
            _context.Update(entity);
        }
        //
        public async Task<T> Get<T>(object id) where T : class
        {
            T item = null;
            item = await _context.Set<T>().FindAsync(id);

            return item;
        }
        public async Task<IEnumerable<T>> GetAll<T>() where T : class
        {

            var item = await _context.Set<T>().ToListAsync();

            return item;
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

            // if (!filterParams.containNotActive)
            // {
            //     purchaseOrders = purchaseOrders.Where(a => a.Status == true);
            // }


            var purchaseOrder = await purchaseOrders
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Supplier)
            .Include(a => a.BusinessPlace)
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

            // if (!filterParams.containNotActive)
            // {
            //     purchaseOrders = purchaseOrders.Where(a => a.Status == true);
            // }

            return await purchaseOrders.Where(a => a.IsDeleted == false).Include(a => a.Supplier).Include(a => a.BusinessPlace).ToListAsync();
        }

        public async Task CreatePurchaseOrder(PurchaseOrderHeader purchaseOrderHeader)
        {
            var supID = (int)purchaseOrderHeader.SupplierId;
            purchaseOrderHeader.Supplier = await GetSupplier(supID);

            var maxId = _context.PurchaseOrderHeaders.Max(model => model.PONumber);
            purchaseOrderHeader.PONumber = maxId + 1;
            // purchaseOrderHeader.Status = false;

            foreach (var pod in purchaseOrderHeader.PurchaseOrderDetail)
            {
                pod.Item = await _context.Items.FirstOrDefaultAsync(a => a.Id == pod.Item.Id);
            }

            Add(purchaseOrderHeader);
            // await SaveAll();

        }

        public async Task<Supplier> GetSupplier(int id)
        {
            var supplier = await _context.Suppliers.Where(a => a.IsDeleted == false).FirstOrDefaultAsync(a => a.Id == id);

            return supplier;
        }

        public async Task<IEnumerable<Supplier>> GetSuppliers()
        {
            var suppliers = await _context.Suppliers.Where(a => a.IsDeleted == false).ToListAsync();

            return suppliers;
        }

        public async Task<Item> GetItem(int id)
        {
            var item = await _context.Items.Where(a => a.IsDeleted == false).Include(a => a.ItemCategory).Include(b => b.Unit).FirstOrDefaultAsync(a => a.Id == id);

            return item;
        }

        public async Task<IEnumerable<Item>> GetItems()
        {
            var items = await _context.Items.Where(a => a.IsDeleted == false).Include(a => a.ItemCategory).Include(b => b.Unit).ToListAsync();

            return items;
        }

        public async Task<GRNHeader> GetGRN(int purchaseOrderId)
        {
            var grn = await _context.GRNHeaders
           .Include(a => a.PurchaseOrderHeader)
           .Include(p => p.GRNDetails).ThenInclude(pd => pd.Item)
           .FirstOrDefaultAsync(a => a.PurchaseOrderHeaderId == purchaseOrderId);

            return grn;
        }

        public async Task<IEnumerable<GRNHeader>> GetGRNs()
        {
            var grns = await _context.GRNHeaders
           .Include(a => a.PurchaseOrderHeader).ToListAsync();

            return grns;
        }

        public async Task CreateGRN(GRNHeader gRNHeader)
        {
            await _context.AddRangeAsync(gRNHeader);
        }
    }
}