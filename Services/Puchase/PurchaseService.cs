using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

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

        public async Task<Purchase> AddPurchaseAsync(PurchaseDto dto, string userId)
        {
            // 기본 유효성 체크
            if (dto.Purchase_or_Sale == "매출" && string.IsNullOrWhiteSpace(dto.Customer_Name))
                throw new ArgumentException("고객사명을 입력해주세요.");

            if (dto.Purchased_Date == null)
                throw new ArgumentException("거래일을 입력해주세요.");

            if (dto.Purchase_Price == null || dto.Purchase_Amount == null)
                throw new ArgumentException("가격과 수량을 입력해주세요.");
            // 매입인 경우 수금 관련 필드는 무시
            if (dto.Purchase_or_Sale == "매입")
            {
                dto.Is_Payment = null;
                dto.Paid_Payment = null;
                dto.Payment_Period_Start = null;
                dto.Payment_Period_End = null;
            }

            var exists = await _context.Purchases.AnyAsync(p =>
                p.Customer_Name == dto.Customer_Name
                && p.Purchased_Date == dto.Purchased_Date
                && p.Product_Name == dto.Product_Name
            );

            if (exists)
                throw new InvalidOperationException(
                    "같은 고객, 날짜, 제품의 구매 내역이 이미 존재합니다."
                );

            if (!int.TryParse(userId, out var createdById))
                throw new UnauthorizedAccessException("작성자 정보를 가져올 수 없습니다.");

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
                CreatedByUserId = createdById,
            };

            await _context.Purchases.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }
    }
}
