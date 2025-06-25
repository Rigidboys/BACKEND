using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using RigidboysAPI.Services.Auth; // ✅ LoginAttemptService 네임스페이스

public class UserService
{
    private readonly AppDbContext _context;
    private readonly LoginAttemptService _loginAttemptService;

    public UserService(AppDbContext context, LoginAttemptService loginAttemptService)
    {
        _context = context;
        _loginAttemptService = loginAttemptService;
    }

    public async Task RegisterAsync(UserDto dto)
    {
        bool exists = await _context.Users.AnyAsync(u => u.UserId == dto.UserId);
        if (exists)
            throw new InvalidOperationException("이미 존재하는 사용자입니다.");

        var user = new User
        {
            Name = dto.Name,
            UserId = dto.UserId,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Email = dto.Email,
            Phone = dto.Phone,
            Role = "User",
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<string> LoginAsync(LoginDto dto, JwtService jwtService)
    {
        var userId = dto.UserId;

        // ✅ 잠금 여부 확인
        if (await _loginAttemptService.IsLockedAsync(userId))
        {
            throw new UnauthorizedAccessException(
                "로그인 시도 횟수 초과. 잠시 후 다시 시도하세요."
            );
        }

        var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            await _loginAttemptService.RecordFailureAsync(userId);
            throw new UnauthorizedAccessException("존재하지 않는 사용자입니다.");
        }

        // ✅ 비밀번호 불일치 처리
        if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
        {
            await _loginAttemptService.RecordFailureAsync(userId);
            throw new UnauthorizedAccessException("비밀번호가 올바르지 않습니다.");
        }

        // ✅ 로그인 성공 → 기록 제거
        await _loginAttemptService.ClearAsync(userId);

        Console.WriteLine($"{user.UserId}가 로그인 하였습니다.");
        return jwtService.GenerateToken(user.Id, user.Role);
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User> FindByUserIdAsync(string userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.UserId == userId)
            ?? throw new InvalidOperationException("사용자를 찾을 수 없습니다.");
    }
}
