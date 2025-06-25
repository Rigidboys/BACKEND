using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/purchases/mutation")]
    public class PurchaseMutationController : ControllerBase
    {
        private readonly PurchaseMutationService _mutationService;

        public PurchaseMutationController(PurchaseMutationService mutationService)
        {
            _mutationService = mutationService;
        }

        // ✅ 매입 / 매출 정보 수정
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "매입 / 매출의 정보를 수정합니다.",
            Tags = new[] { "매입 / 매출 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "수정 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(404, "매입 / 매출 정보를 찾을 수 없음")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Update(int id, [FromBody] PurchaseDto dto)
        {
            try
            {
                await _mutationService.UpdateAsync(id, dto);
                return Ok("매입 / 매출 수정 완료");
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
                    ErrorCodes.DUPLICATE_CUSTOMER,
                    ErrorCodes.DUPLICATE_CUSTOMER_MESSAGE
                );
            }
            catch (Exception ex) //500
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }

        // ✅ 고객 정보 삭제
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "매입 / 매출의 정보를 삭제합니다.",
            Tags = new[] { "매입 / 매출 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "삭제 성공")]
        [SwaggerResponse(404, "매입 / 매출 정보를 찾을 수 없음")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _mutationService.DeleteAsync(id);
                return Ok(new { message = "매입 / 매출 정보 삭제 완료" });
            }
            catch (InvalidOperationException ex) //404
            {
                return NotFound(new { code = ErrorCodes.PURCHASE_NOT_FOUND, message = ex.Message });
            }
            catch (Exception ex) //500
            {
                return ErrorResponseHelper.HandleServerError(
                    ErrorCodes.PURCHASE_NOT_FOUND_MESSAGE,
                    ex.Message
                );
            }
        }
    }
}
