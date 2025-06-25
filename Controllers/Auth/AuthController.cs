using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Models;
using Swashbuckle.AspNetCore.Annotations;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtService _jwtService;

    public AuthController(UserService userService, JwtService jwtService)
    {
        _userService = userService;
        _jwtService = jwtService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    [SwaggerOperation(
        Summary = "로그인",
        Description = "아이디와 비밀번호를 이용하여 로그인합니다.",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    [SwaggerResponse(200, "로그인 성공", typeof(object))] // 반환 데이터 타입 명시
    [SwaggerResponse(401, "아이디 또는 비밀번호가 잘못되었습니다.")]
    [SwaggerResponse(429, "로그인 시도 횟수 초과로 잠시 로그인 제한됨")]
    [SwaggerResponse(500, "서버 오류")]
    public async Task<IActionResult> Login([FromBody] LoginDto dto)
    {
        try
        {
            var token = await _userService.LoginAsync(dto, _jwtService);
            var user = await _userService.FindByUserIdAsync(dto.UserId);

            Console.WriteLine($"{user.UserId}가 로그인 하였습니다.");

            return Ok(new
            {
                token,
                userId = user.UserId,
                name = user.Name,
                role = user.Role
            });
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine($"[로그인 실패] 사유: {e.Message}");
            return Unauthorized(new { message = e.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"서버 오류: {ex.Message}" });
        }
    }
    //비밀번호 제외해서 받아오도록
    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    [SwaggerOperation(
        Summary = "회원가입된 사용자 목록 조회 (관리자 전용)",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    [SwaggerResponse(200, "조회 성공", typeof(List<User>))]
    [SwaggerResponse(403, "권한 없음")]
    public async Task<ActionResult<List<User>>> GetAll()
    {
        var result = await _userService.GetAllAsync();
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [SwaggerOperation(
        Summary = "회원가입",
        Tags = new[] { "로그인 및 회원가입" }
    )]
    [SwaggerResponse(200, "회원가입 성공")]
    [SwaggerResponse(409, "중복된 사용자 ID")]
    [SwaggerResponse(400, "입력값 오류")]
    [SwaggerResponse(500, "서버 오류")]
    public async Task<IActionResult> Register([FromBody] UserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(new { message = "입력값이 유효하지 않습니다." });

        try
        {
            await _userService.RegisterAsync(dto);
            return Ok(new { message = "회원가입 완료" });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = $"서버 오류: {ex.Message}" });
        }
    }
}
