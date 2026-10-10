using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProductRespository(StoreContext db) : IProductRepository
{
    public void AddProduct(Product product)
    {
        db.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
        db.Products.Remove(product);
    }

    public async Task<IReadOnlyList<string>> GetBrandsAsync()
    {
        return await db.Products.Select(x => x.Brand).Distinct().ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await db.Products.FindAsync(id);
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync(string? brand, string? type, string? sort)
    {
        var query = db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(brand))
        {
            query = query.Where(x => x.Brand == brand);
        }
        if (!string.IsNullOrWhiteSpace(type))
        {
            query = query.Where(x => x.Type == type);
        }

        query = sort switch
        {
            "priceAsc" => query.OrderBy(x => x.Price),
            "priceDesc" => query.OrderByDescending(x => x.Price),
            _ => query.OrderBy(x => x.Name)
        };

        return await query.ToListAsync();
    }


    public async Task<IReadOnlyList<string>> GetTypesAsync()
    {
        return await db.Products.Select(x => x.Type).Distinct().ToListAsync();
    }

    public bool ProductExists(int id)
    {
        return db.Products.Any(x => x.ProductId == id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await db.SaveChangesAsync() > 0;
    }

    public void UpdateProduct(Product product)
    {
        db.Entry(product).State = EntityState.Modified;
    }
}
