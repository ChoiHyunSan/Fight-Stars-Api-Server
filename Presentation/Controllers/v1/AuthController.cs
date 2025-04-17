using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterLocalAccount([FromBody] RegisterRequest request)
    {
        var registerUser = await _authService.RegisterWithLocalAsync(request.UserName, request.Email, request.Password);
        return Ok(AuthConverter.toRegisterResponse(registerUser));
    }

    [HttpPost("login")]
    public Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminAction()
    {
        return Ok("관리자만 볼 수 있는 내용입니다");
    }
}

