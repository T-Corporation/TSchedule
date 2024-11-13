using Microsoft.EntityFrameworkCore;
using TSchedule.Persistence.Entities;
using TSchedule.Persistence.Interfaces;

namespace TSchedule.Persistence.Services;

public class ProductsService : IProductsService
{
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        await using ApplicationDbContext context = new();
        return await context.Products.AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductByLikeName(string name)
    {
        await using ApplicationDbContext context = new();
        return await context.Products.AsNoTracking()
            .Where(p => EF.Functions.Like(p.Name, $"%{name}%"))
            .ToListAsync();
    }

    public async Task<Product?> GetProductByName(string name)
    {
        await using ApplicationDbContext context = new();
        return await context.Products.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Name == name);
    }
}
