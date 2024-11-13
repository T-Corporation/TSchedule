using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces.Bases;

namespace TSchedule.Persistence.Interfaces;

public interface IProductsService : IService
{
    Task<Product?> GetProductByName(string name);
    Task<IEnumerable<Product>> GetAllProducts();
    Task<IEnumerable<Product>> GetProductByLikeName(string name);
}
