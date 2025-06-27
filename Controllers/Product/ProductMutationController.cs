using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;
using RigidboysAPI.Errors;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/products/mutation")]
    public class ProductMutationController : ControllerBase
    {
        private readonly ProductMutationService _mutationService;

        public ProductMutationController(ProductMutationService mutationService)
        {
            _mutationService = mutationService;
        }

        [HttpPut("{productName}")]
        [SwaggerOperation(Summary = "제품의 정보를 수정합니다.", Tags = new[] { "제품 관리" })]
        public async Task<IActionResult> Update(string productName, [FromBody] ProductDto dto)
        {
            try
            {
                await _mutationService.UpdateAsync(productName, dto);
                return Ok("제품 정보 수정 완료");
            }
            catch (ArgumentException)
            {
                return ErrorResponseHelper.HandleBadRequest(ErrorCodes.INVALID_INPUT, ErrorCodes.INVALID_INPUT_MESSAGE);
            }
            catch (InvalidOperationException)
            {
                return ErrorResponseHelper.HandleConflict(ErrorCodes.DUPLICATE_CUSTOMER, ErrorCodes.DUPLICATE_CUSTOMER_MESSAGE);
            }
            catch (Exception ex)
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }

        [HttpDelete("{productName}")]
        [SwaggerOperation(Summary = "제품을 삭제합니다.", Tags = new[] { "제품 관리" })]
        public async Task<IActionResult> Delete(string productName)
        {
            try
            {
                await _mutationService.DeleteAsync(productName);
                return Ok(new { message = "제품 삭제 완료" });
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { code = ErrorCodes.PRODUCT_NOT_FOUND, message = ex.Message });
            }
            catch (Exception ex)
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.PRODUCT_NOT_FOUND_MESSAGE, ex.Message);
            }
        }
    }
}
