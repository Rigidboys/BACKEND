using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Models;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RigidboysAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/customers/mutation")]
    public class CustomerMutationController : ControllerBase
    {
        private readonly CustomerMutationService _mutationService;

        public CustomerMutationController(CustomerMutationService mutationService)
        {
            _mutationService = mutationService;
        }

        // ✅ 고객 정보 수정
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "고객 정보 수정", Tags = new[] { "고객 관리" })]
        [SwaggerResponse(200, "수정 성공", typeof(Customer))]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(404, "고객을 찾을 수 없음")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Update(int id, [FromBody] CustomerDto dto)
        {
            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });

                var updatedCustomer = await _mutationService.UpdateAsync(id, dto, role, userId);
                return Ok(updatedCustomer);
            }
            catch (ArgumentException)
            {
                return ErrorResponseHelper.HandleBadRequest(
                    ErrorCodes.INVALID_INPUT,
                    ErrorCodes.INVALID_INPUT_MESSAGE
                );
            }
            catch (InvalidOperationException)
            {
                return ErrorResponseHelper.HandleConflict(
                    ErrorCodes.DUPLICATE_CUSTOMER,
                    ErrorCodes.DUPLICATE_CUSTOMER_MESSAGE
                );
            }
            catch (Exception ex)
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }

        // ✅ 고객 정보 삭제
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "고객 정보 삭제", Tags = new[] { "고객 관리" })]
        [SwaggerResponse(200, "삭제 성공")]
        [SwaggerResponse(404, "고객을 찾을 수 없음")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var userId = User.FindFirst("UserId")?.Value;

                if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
                    return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });
                var deleted = await _mutationService.DeleteAsync(id, role, userId);
                return Ok(new { message = "고객 삭제 완료" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { code = ErrorCodes.CUSTOMER_NOT_FOUND, message = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorResponseHelper.HandleServerError(
                    ErrorCodes.CUSTOMER_NOT_FOUND_MESSAGE,
                    ex.Message
                );
            }
        }
    }
}
