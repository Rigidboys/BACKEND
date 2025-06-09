using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class ProductService
    {
        private readonly AppDbContext _context;

        public ProductService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task AddCustomerAsync(ProductDto dto)
        {
            var entity = new Product
            {
                Product_name = dto.Product_name,
                Category = dto.Category,
                License = dto.License,
                Product_price = dto.Product_price,
                Description = dto.Description
            };

            _context.Products.Add(entity);
            await _context.SaveChangesAsync();
        }
    }
}
