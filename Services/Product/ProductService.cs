using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

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

        public async Task<Product> AddProductAsync(ProductDto dto)
        {
            bool exists = await _context.Products.AnyAsync(p => p.Product_Name == dto.Product_Name);
            if (exists)
            {
                throw new InvalidOperationException("이미 등록된 제품명입니다.");
            }

            var entity = new Product
            {
                Product_Name = dto.Product_Name,
                Category = dto.Category,
                License = dto.License,
                Product_price = dto.Product_price,
                Production_price = dto.Production_price,
                Description = dto.Description,
            };

            _context.Products.Add(entity);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // ✅ DB 제약 조건 오류를 프론트에서 확인할 수 있게 전달
                throw new Exception("제품 등록 중 오류: " + ex.InnerException?.Message);
            }

            return entity;
        }

        public async Task<List<string>> GetProductNamesAsync()
        {
            return await _context.Products
                .Select(p => p.Product_Name)
                .Distinct()
                .ToListAsync();
        }
    }
}
