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
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        [SwaggerOperation(
            Summary = "제품 목록 조회",
            Tags = new[] { "제품 관리" }
        )]
        [SwaggerResponse(200, "조회 성공", typeof(List<Product>))]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "제품을 추가합니다.",
            Tags = new[] { "제품 관리" }
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 제품")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            // ✅ 유효성 검사 실패 시 BadRequest 반환
            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);

            try
            {
                var savedProduct = await _service.AddProductAsync(dto);
                return Ok(savedProduct);
            }
            catch (ArgumentException ex)
            {
                // ✅ 예외 메시지를 그대로 전달 (개발 시 디버깅 유용)
                return ErrorResponseHelper.HandleBadRequest(
                    ErrorCodes.INVALID_INPUT,
                    ex.Message
                );
            }
            catch (InvalidOperationException ex)
            {
                // ✅ 중복된 제품 등록 시 충돌 응답
                return ErrorResponseHelper.HandleConflict(
                    ErrorCodes.DUPLICATE_PRODUCT,
                    ex.Message
                );
            }
            catch (Exception ex)
            {
                // ✅ 모든 예외를 ObjectResult 기반으로 반환하여 CORS 대응
                return ErrorResponseHelper.HandleServerError(
                    ErrorCodes.SERVER_ERROR,
                    ex.Message // ✅ message에 실제 예외 메시지 전달
                );
            }
        }

        [HttpGet("names")]
        [SwaggerOperation(
            Summary = "제품의 이름만 조회합니다.",
            Tags = new[] { "제품 관리" }
        )]
        [SwaggerResponse(200, "제품의 이름 목록 조회 성공", typeof(List<string>))]
        public async Task<ActionResult<List<string>>> GetProductNames()
        {
            var names = await _service.GetProductNamesAsync();
            return Ok(names);
        }
    }
}
