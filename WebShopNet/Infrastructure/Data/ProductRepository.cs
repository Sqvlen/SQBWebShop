using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class ProductRepository : IProductRepository
{
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }
    
    public async Task<Product> GetProductByIdAsync(int id)
    {
        var product = await _context.Products.FindAsync(id);
        if (product != null)
            return product;
        else
            throw new NotImplementedException("The product doesn't exist");
    }

    public async Task<IReadOnlyList<Product>> GetProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
    {
        return await _context.ProductBrands.ToListAsync();
    }

    public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
    {
        return await _context.ProductTypes.ToListAsync();
    }
}