using System.Collections.Generic;
using System.Threading.Tasks;
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
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }

        public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<PurchaseOrderHeader> GetPurchaseOrder(int id)
        {
            var purchaseOrder = await _context.PurchaseOrderHeaders
            .Include(p => p.PurchaseOrderDetail).ThenInclude(pd => pd.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

            return purchaseOrder;
            // throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<PurchaseOrderHeader>> GetPurchaseOrders()
        {
            var purchaseOrders = await _context.PurchaseOrderHeaders.ToListAsync();

            return purchaseOrders;
        }
    }
}