using Microsoft.EntityFrameworkCore; 
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;

namespace RigidboysAPI.Services.Auth
{
    public class LoginAttemptService
    {
        private readonly AppDbContext _context;

        public LoginAttemptService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LoginAttempt?> GetAttemptAsync(string userId)
        {
            return await _context.LoginAttempts.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        public async Task RecordFailureAsync(string userId)
        {
            var now = DateTime.UtcNow;
            var attempt = await GetAttemptAsync(userId);

            if (attempt == null)
            {
                _context.LoginAttempts.Add(
                    new LoginAttempt
                    {
                        UserId = userId,
                        FailedAttempts = 1,
                        LastAttemptAt = now,
                    }
                );
            }
            else
            {
                attempt.FailedAttempts++;
                attempt.LastAttemptAt = now;

                if (attempt.FailedAttempts >= 5)
                    attempt.LockedUntil = now.AddSeconds(30);

                _context.LoginAttempts.Update(attempt);
            }

            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsLockedAsync(string userId)
        {
            var attempt = await GetAttemptAsync(userId);
            return attempt != null
                && attempt.LockedUntil != null
                && DateTime.UtcNow < attempt.LockedUntil;
        }

        public async Task ClearAsync(string userId)
        {
            var attempt = await GetAttemptAsync(userId);
            if (attempt != null)
            {
                _context.LoginAttempts.Remove(attempt);
                await _context.SaveChangesAsync();
            }
        }
    }
}
