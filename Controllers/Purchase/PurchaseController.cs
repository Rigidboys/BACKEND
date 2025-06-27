using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Models;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using RigidboysAPI.Data;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/purchases")]
    public class PurchaseController : ControllerBase
    {
        private readonly PurchaseService _service;

        public PurchaseController(PurchaseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Purchase>>> GetAll()
        {
            try
            {
                var role = User.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
                {
                    Console.WriteLine("❗인증 정보 누락!");
                    return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });
                }

                var result = await _service.GetAllAsync(role, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❗[GetAll 에러]: {ex.Message}");
                return StatusCode(500, new { message = "서버 내부 오류 발생", detail = ex.Message });
            }
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "매입 / 매출을 등록합니다.",
            Tags = new[] { "매입 / 매출 관리" }
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 매입 / 매출")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] PurchaseDto dto)
        {
            Console.WriteLine("🟢 [Create 시작] dto: " + JsonSerializer.Serialize(dto));
            if (!ModelState.IsValid)
                Console.WriteLine("⚠️ 유효성 검사 실패: " + JsonSerializer.Serialize(ModelState));

            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);

            var userId = User.FindFirst("UserId")?.Value;
            Console.WriteLine("🔒 userId: " + userId);
            if (string.IsNullOrEmpty(userId))
            {
                Console.WriteLine("❗ 인증정보 누락");
                return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });
            }

            try
            {
                var saved = await _service.AddPurchaseAsync(dto, userId);
                Console.WriteLine("✅ 저장 완료: Id=" + saved.Id);
                return Ok(saved);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("🚫 ArgumentException: " + ex.Message);
                return ErrorResponseHelper.HandleBadRequest(ErrorCodes.INVALID_INPUT, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("🚫 InvalidOperationException: " + ex.Message);
                return ErrorResponseHelper.HandleConflict(ErrorCodes.DUPLICATE_PURCHASE, ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("🔥 Exception: " + ex.Message + "\n" + ex.StackTrace);
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }
    }
}
