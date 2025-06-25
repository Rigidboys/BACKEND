using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _context;

        public CustomerService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync()
        {
            return await _context.Customers.ToListAsync();
        }

        public async Task<Customer> AddCustomerAsync(CustomerDto dto, string userId)
        {
            bool exists = await _context.Customers.AnyAsync(c => c.Office_Name == dto.Office_Name);
            if (exists)
            {
                throw new InvalidOperationException("이미 등록된 고객사입니다.");
            }

            var entity = new Customer
            {
                Office_Name = dto.Office_Name,
                Type = dto.Type,
                Master_Name = dto.Master_Name,
                Phone = dto.Phone,
                Address = dto.Address,
                Description = dto.Description,
                CreatedByUserId = int.Parse(userId),
            };

            _context.Customers.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<List<Customer>> GetAllAsync(string role, string userId)
        {
            var query = _context.Customers.AsQueryable();

            if (role != "Admin")
                query = query.Where(c => c.CreatedByUserId == int.Parse(userId));

            return await query.ToListAsync();
        }

        // ✅ 여기에 role과 userId 반영된 버전 추가
        public async Task<List<string>> GetCustomerNamesAsync(string role, string userId)
        {
            var query = _context.Customers.AsQueryable();

            if (role != "Admin")
                query = query.Where(c => c.CreatedByUserId == int.Parse(userId));

            return await query.Select(c => c.Office_Name).Distinct().ToListAsync();
        }
    }
}
