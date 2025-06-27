using RigidboysAPI.Data;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;
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

        public async Task<Purchase> AddPurchaseAsync(PurchaseDto dto, string userId)
        {
            Console.WriteLine($"▶️ [AddPurchaseAsync 시작] dto: {System.Text.Json.JsonSerializer.Serialize(dto)}");

            if (dto.Purchase_or_Sale == "매출")
            {
                if (string.IsNullOrWhiteSpace(dto.Customer_Name))
                {
                    Console.WriteLine("❌ 고객사명이 비어 있음");
                    throw new ArgumentException("고객사명을 입력해주세요.");
                }
            }
            else if (dto.Purchase_or_Sale == "매입")
            {
                if (string.IsNullOrWhiteSpace(dto.Seller_Name))
                {
                    Console.WriteLine("❌ 판매자명 누락 - 매입일 경우 필수");
                    throw new ArgumentException("매입 거래는 판매자명을 반드시 입력해야 합니다.");
                }
                dto.Customer_Name = null; // 매입은 고객사명 null 처리
            }
            else
            {
                throw new ArgumentException("매출 또는 매입 중 하나를 선택해주세요.");
            }

            if (dto.Purchased_Date == null)
            {
                Console.WriteLine("❌ 거래일 누락");
                throw new ArgumentException("거래일을 입력해주세요.");
            }

            if (dto.Purchase_Price == null || dto.Purchase_Amount == null)
            {
                Console.WriteLine("❌ 가격/수량 누락");
                throw new ArgumentException("가격과 수량을 입력해주세요.");
            }

            var exists = await _context.Purchases.AnyAsync(p =>
                p.Purchase_or_Sale == dto.Purchase_or_Sale &&
                p.Purchased_Date == dto.Purchased_Date &&
                p.Product_Name == dto.Product_Name
            );

            if (exists)
            {
                Console.WriteLine("⚠️ 중복 거래 발견");
                throw new InvalidOperationException("같은 거래 내역이 이미 존재합니다.");
            }

            if (!int.TryParse(userId, out var createdById))
            {
                Console.WriteLine("❌ 유저ID 파싱 실패");
                throw new UnauthorizedAccessException("작성자 정보를 가져올 수 없습니다.");
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
                CreatedByUserId = createdById,
            };

            try
            {
                await _context.Purchases.AddAsync(entity);
                await _context.SaveChangesAsync();
                Console.WriteLine($"✅ 저장 성공: Purchase ID = {entity.Id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"🔥 DB 저장 실패: {ex.Message}\n{ex.StackTrace}");
                throw;
            }

            return entity;
        }
    }
}
