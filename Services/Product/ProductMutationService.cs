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

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id); // ✅ 복수형으로 수정
            if (product == null)
                throw new InvalidOperationException("삭제할 고객사가 없습니다.");

            _context.Products.Remove(product); // ✅ 변수 이름도 수정
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, ProductDto dto)
        {
            var product = await _context.Products.FindAsync(id); // ✅ 복수형으로 수정
            if (product == null)
                throw new InvalidOperationException("수정할 고객사가 없습니다.");

            product.Product_Name = dto.Product_Name;
            product.Category = dto.Category;
            product.License = dto.License;
            product.Product_price = dto.Product_price;
            product.Production_price = dto.Production_price; // ✅ 실수 수정
            product.Description = dto.Description;

            _context.Products.Update(product); // ✅ 복수형으로 수정
            await _context.SaveChangesAsync();
        }
    }
}
