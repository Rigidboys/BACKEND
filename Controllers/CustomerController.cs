using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Services;
using RigidboysAPI.Models;
using RigidboysAPI.Dtos;

namespace RigidboysAPI.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _service;

        public CustomerController(CustomerService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<Customer>>> GetAll()
        {
            return await _service.GetAllAsync();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CustomerDto dto)
        {
            await _service.AddCustomerAsync(dto);
            return Ok(new { message = "신규 고객사를 저장했습니다." });
        }
        
    }
}
