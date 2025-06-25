using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RigidboysAPI.Dtos;
using RigidboysAPI.Errors;
using RigidboysAPI.Models;
using RigidboysAPI.Services;
using Swashbuckle.AspNetCore.Annotations;

[Authorize]
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
    [SwaggerOperation(Summary = "고객 목록 조회", Tags = new[] { "고객 관리" })]
    [SwaggerResponse(200, "조회 성공", typeof(List<Customer>))]
    public async Task<ActionResult<List<Customer>>> GetAll()
    {
        var role = User.FindFirst(
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        )?.Value!;
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });

        var result = await _service.GetAllAsync(role!, userId);
        return Ok(result);
    }

    [HttpPost]
    [SwaggerOperation(Summary = "고객을 등록합니다.", Tags = new[] { "고객 관리" })]
    [SwaggerResponse(200, "등록 성공")]
    [SwaggerResponse(400, "입력값 오류")]
    [SwaggerResponse(409, "중복된 고객")]
    [SwaggerResponse(500, "서버 오류")]
    public async Task<IActionResult> Create([FromBody] CustomerDto dto)
    {
        if (!ModelState.IsValid)
            return ErrorResponseHelper.HandleBadRequest(ModelState);

        var userId = User.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });
        try
        {
            var savedCustomer = await _service.AddCustomerAsync(dto, userId);
            return Ok(savedCustomer);
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

    [HttpGet("Office_Name")]
    [SwaggerOperation(Summary = "고객사 이름만 조회합니다.", Tags = new[] { "고객 관리" })]
    [SwaggerResponse(200, "고객사의 이름 목록 조회 성공", typeof(List<string>))]
    public async Task<ActionResult<List<string>>> GetCustomerNames()
    {
        var role = User.FindFirst(
            "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
        )?.Value;
        var userId = User.FindFirst("UserId")?.Value;

        if (string.IsNullOrEmpty(role) || string.IsNullOrEmpty(userId))
            return Unauthorized(new { message = "인증 정보가 유효하지 않습니다." });

        var names = await _service.GetCustomerNamesAsync(role, userId);
        return Ok(names);
    }
}
