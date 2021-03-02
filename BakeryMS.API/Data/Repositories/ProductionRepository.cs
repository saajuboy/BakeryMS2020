using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BakeryMS.API.Data.Interfaces;
using BakeryMS.API.Models.Production;
using Microsoft.EntityFrameworkCore;

namespace BakeryMS.API.Data.Repositories
{
    public class ProductionRepository : IProductionRepository
    {
        private readonly DataContext _context;

        public ProductionRepository(DataContext context)
        {
            _context = context;

        }
        public void Add<T>(T entity) where T : class
        {
            _context.AddRangeAsync(entity);
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

        public async Task<ProductionOrderHeader> GetProductionOrder(int id)
        {
            var productionOrder = await _context.ProductionOrderHeaders
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Session)
            .Include(a => a.BusinessPlace)
            .Include(a => a.User)
            .Include(a => a.ProductionOrderDetails).ThenInclude(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

            return productionOrder;
        }

        public async Task<IEnumerable<ProductionOrderHeader>> GetProductionOrders()
        {
            var prodOrders = await _context.ProductionOrderHeaders
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Session)
            .Include(a => a.User)
            .Include(a => a.BusinessPlace).ToListAsync();

            return prodOrders;
        }

        public async Task CreateProductionOrder(ProductionOrderHeader productionOrderHeader)
        {
            var maxId = await _context.ProductionOrderHeaders.MaxAsync(model => model.ProductionOrderNo);

            productionOrderHeader.ProductionOrderNo = maxId + 1;

            Add(productionOrderHeader);
        }

        public async Task<IngredientHeader> GetIngredient(int id)
        {
            var ingredient = await _context.IngredientHeaders
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Item)
            .Include(a => a.IngredientsDetail).ThenInclude(a => a.Item)
            .FirstOrDefaultAsync(a => a.Id == id);

            return ingredient;
        }

        public async Task<IEnumerable<IngredientHeader>> GetIngredients()
        {
            var ingredients = await _context.IngredientHeaders
            .Where(a => a.IsDeleted == false)
            .Include(a => a.Item)
            .ToListAsync();

            return ingredients;
        }

        public async Task CreateIngredient(IngredientHeader ingredientHeader)
        {
            var ingredient = await _context.AddAsync(ingredientHeader);
        }
    }
}