using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


/***************************

       AuthController

***************************/
// Description
// : 인증 관련 API를 제공하는 컨트롤러입니다.
//   회원가입, 로그인, JWT 토큰 갱신 등의 기능을 제공합니다.
//   JWT 인증을 사용하여 보호된 리소스에 대한 액세스를 제어합니다.
//
// Author : ChoiHyunSan

[Route("api/auth")]
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
        var registerUser = await _authService.RegisterAsync(request.UserName, request.Email, request.Password);
        return Ok(AuthConverter.toRegisterResponse(registerUser));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.Login(request.Username, request.Password);
        return Ok(response);
    }

    [HttpPost("login/save")]
    [Authorize]
    public Task<IActionResult> SaveLogin()
    {
        return Task.FromResult<IActionResult>(Ok());
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshRequest request)
    {
        var response = await _authService.Refresh(request.RefreshToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("jwt")]
    public IActionResult JwtTest()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var role = User.FindFirstValue(ClaimTypes.Role);

        return Ok(new
        {
            Message = "🔐 로그인된 사용자 정보입니다.",
            UserId = userId,
            Role = role
        });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("admin-only")]
    public IActionResult AdminAction()
    {
        return Ok("관리자만 볼 수 있는 내용입니다");
    }
}

