using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Models;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations; // ✅ 이거 추가해야 Swagger 주석 작동

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
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "조회 성공", typeof(List<Product>))]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            var result = await _service.GetAllAsync(); // ✅ 수정된 부분
            return Ok(result);
        }

        [HttpPost]
        [SwaggerOperation(
            Summary = "제품을 추가합니다.",
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "등록 성공")]
        [SwaggerResponse(400, "입력값 오류")]
        [SwaggerResponse(409, "중복된 제품")]
        [SwaggerResponse(500, "서버 오류")]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            if (!ModelState.IsValid)
                return ErrorResponseHelper.HandleBadRequest(ModelState);
            try
            {
                var savedProduct = await _service.AddProductAsync(dto);
                return Ok(savedProduct); // ✅ 제품 객체를 응답으로 보냄
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
                    ErrorCodes.DUPLICATE_PRODUCT,
                    ErrorCodes.DUPLICATE_PRODUCT_MESSAGE
                );
            }
            catch (Exception ex)
            {
                return ErrorResponseHelper.HandleServerError(ErrorCodes.SERVER_ERROR, ex.Message);
            }
        }


        [HttpGet("names")]
        [SwaggerOperation(
            Summary = "제품의 이름만 조회합니다.",
            Tags = new[] { "제품 관리" } // ✅ 여기에 태그 입력
        )]
        [SwaggerResponse(200, "제품의 이름 목록 조회 성공", typeof(List<string>))]
        public async Task<ActionResult<List<string>>> GetProductNames()
        {
            var names = await _service.GetProductNamesAsync();
            return Ok(names);
        }
    }
}
