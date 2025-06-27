using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Services
{
    public class PurchaseMutationService
    {
        private readonly AppDbContext _context;

        public PurchaseMutationService(AppDbContext context)
        {
            _context = context;
        }

        // 삭제
        public async Task DeleteAsync(int id)
        {
            var purchase = await _context.Purchases.FindAsync(id); // ✅ Purchases로 수정
            if (purchase == null)
                throw new InvalidOperationException("삭제할 매입/매출 정보가 없습니다.");

            _context.Purchases.Remove(purchase); // ✅ 변수명 일치
            await _context.SaveChangesAsync();
        }

        // 수정
        public async Task UpdateAsync(int id, PurchaseDto dto)
        {
            var purchase = await _context.Purchases.FindAsync(id); // ✅ Purchases로 수정
            if (purchase == null)
                throw new InvalidOperationException("수정할 매입/매출 정보가 없습니다.");

            // dto 값 복사
            purchase.Purchase_or_Sale = dto.Purchase_or_Sale;
            purchase.Customer_Name = dto.Customer_Name;
            purchase.Purchased_Date = dto.Purchased_Date;
            purchase.Product_Name = dto.Product_Name;
            purchase.Purchase_Amount = dto.Purchase_Amount;
            purchase.Purchase_Price = dto.Purchase_Price;
            purchase.Payment_Period_Deadline = dto.Payment_Period_Deadline;
            purchase.Payment_Period_Start = dto.Payment_Period_Start;
            purchase.Payment_Period_End = dto.Payment_Period_End;
            purchase.Is_Payment = dto.Is_Payment;
            purchase.Description = dto.Description;
            purchase.Paid_Payment = dto.Paid_Payment;
            purchase.Seller_Name = dto.Seller_Name ?? string.Empty;

            _context.Purchases.Update(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
