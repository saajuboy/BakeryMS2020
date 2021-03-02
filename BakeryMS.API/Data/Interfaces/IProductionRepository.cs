using System.Collections.Generic;
using System.Threading.Tasks;
using BakeryMS.API.Models.Production;

namespace BakeryMS.API.Data.Interfaces
{
    public interface IProductionRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        void Update<T>(T entity) where T : class;
        Task<T> Get<T>(object id) where T : class;
        Task<IEnumerable<T>> GetAll<T>() where T : class;
        Task<bool> SaveAll();
        Task<ProductionOrderHeader> GetProductionOrder(int id);
        Task<IEnumerable<ProductionOrderHeader>> GetProductionOrders();
        Task CreateProductionOrder(ProductionOrderHeader productionOrderHeader);
        Task<IngredientHeader> GetIngredient(int id);
        Task<IEnumerable<IngredientHeader>> GetIngredients();
        Task CreateIngredient(IngredientHeader ingredientHeader);

    }
}