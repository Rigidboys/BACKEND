using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Services
{
    public class ProductMutationService
    {
        private readonly AppDbContext _context;

        public ProductMutationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(string productName)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Product_Name == productName);
            if (product == null)
                throw new InvalidOperationException("삭제할 제품이 존재하지 않습니다.");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(string productName, ProductDto dto)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Product_Name == productName);
            if (product == null)
                throw new InvalidOperationException("수정할 제품이 존재하지 않습니다.");

            product.Product_Name = dto.Product_Name;
            product.Category = dto.Category;
            product.License = dto.License;
            product.Product_price = dto.Product_price;
            product.Production_price = dto.Production_price;
            product.Description = dto.Description;

            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
