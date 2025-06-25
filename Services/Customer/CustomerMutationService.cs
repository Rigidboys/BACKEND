using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Services
{
    public class CustomerMutationService
    {
        private readonly AppDbContext _context;

        public CustomerMutationService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Customer> DeleteAsync(int id, string role, string userId)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                throw new InvalidOperationException("삭제할 고객사가 없습니다.");

            if (role != "Admin")
            {
                if (!int.TryParse(userId, out int parsedUserId))
                    throw new UnauthorizedAccessException("유효하지 않은 사용자 정보입니다.");

                if (customer.CreatedByUserId != parsedUserId)
                    throw new UnauthorizedAccessException("권한이 없습니다.");
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<Customer> UpdateAsync(int id, CustomerDto dto, string role, string userId)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
                throw new InvalidOperationException("수정할 고객사가 없습니다.");

            if (role != "Admin")
            {
                if (!int.TryParse(userId, out int parsedUserId))
                    throw new UnauthorizedAccessException("유효하지 않은 사용자 정보입니다.");

                if (customer.CreatedByUserId != parsedUserId)
                    throw new UnauthorizedAccessException("권한이 없습니다.");
            }

            customer.Office_Name = dto.Office_Name;
            customer.Type = dto.Type;
            customer.Master_Name = dto.Master_Name;
            customer.Phone = dto.Phone;
            customer.Address = dto.Address;
            customer.Description = dto.Description;

            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();

            return customer;
        }
    }
}
