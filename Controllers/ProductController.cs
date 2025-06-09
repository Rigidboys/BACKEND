using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/Product")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _service;

        public ProductController(ProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ProductDto dto)
        {
            await _service.AddCustomerAsync(dto);
            return Ok(new { message = "신규 제품정보를 저장했습니다." });
        }
        
    }
}
