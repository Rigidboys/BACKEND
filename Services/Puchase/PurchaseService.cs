using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace RigidboysAPI.Services
{
    public class PurchaseService
    {
        private readonly AppDbContext _context;

        public PurchaseService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Purchase>> GetAllAsync(string role, string userId)
        {
            var query = _context.Purchases.AsQueryable();

            if (role != "Admin")
                query = query.Where(p => p.CreatedByUserId == int.Parse(userId));

            return await query.ToListAsync();
        }

        public async Task AddPurchaseAsync(PurchaseDto dto, string userId)
        {
            var exists = await _context.Purchases.AnyAsync(p =>
                p.Customer_Name == dto.Customer_Name &&
                p.Purchased_Date == dto.Purchased_Date &&
                p.Product_Name == dto.Product_Name);

            if (exists)
            {
                throw new InvalidOperationException("같은 고객, 날짜, 제품의 구매 내역이 이미 존재합니다.");
            }
            var entity = new Purchase
            {
                Purchase_or_Sale = dto.Purchase_or_Sale,
                Customer_Name = dto.Customer_Name,
                Purchased_Date = dto.Purchased_Date,
                Product_Name = dto.Product_Name,
                Purchase_Amount = dto.Purchase_Amount,
                Purchase_Price = dto.Purchase_Price,
                Payment_Period_Deadline = dto.Payment_Period_Deadline,
                Payment_Period_Start = dto.Payment_Period_Start,
                Payment_Period_End = dto.Payment_Period_End,
                Is_Payment = dto.Is_Payment,
                Description = dto.Description,
                Paid_Payment = dto.Paid_Payment,
                Seller_Name = dto.Seller_Name ?? string.Empty,
                CreatedByUserId = int.Parse(userId) // ✅ 작성자 정보 저장
            };

            await _context.Purchases.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

    }
}
