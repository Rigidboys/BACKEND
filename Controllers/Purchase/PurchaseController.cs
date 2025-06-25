using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Models;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

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
        [SwaggerOperation(
            Summary = "매입 / 매출 정보 조회",
            Tags = new[] { "매입 / 매출 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "조회 성공", typeof(List<Purchase>))]
        public async Task<ActionResult<List<Purchase>>> GetAll()
        {
            var role = User.FindFirst(
                "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
            )?.Value;
            var userId = User.FindFirst("UserId")?.Value;

            if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });

            var result = await _service.GetAllAsync(role, userId); // ✅ 변경
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "매입 / 매출을 등록합니다.",
            Tags = new[] { "매입 / 매출 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 매입 / 매출")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] PurchaseDto dto)
        {
            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);

            var userId = User.FindFirst("UserId")?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });
            try
            {
                await _service.AddPurchaseAsync(dto, userId);
                return Ok(new { message = "매입/매출 정보가 등록되었습니다." });
            }
            catch (ArgumentException) //400
            {
                return ErrorResponseHelper.HandleBadRequest(
                    ErrorCodes.INVALID_INPUT,
                    ErrorCodes.INVALID_INPUT_MESSAGE
                );
            }
            catch (InvalidOperationException) //409
            {
                return ErrorResponseHelper.HandleConflict(
                    ErrorCodes.DUPLICATE_PURCHASE,
                    ErrorCodes.DUPLICATE_PURCHASE_MESSAGE
                );
            }
            catch (Exception ex) //500
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }
    }
}
