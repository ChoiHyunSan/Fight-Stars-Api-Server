using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


/// <summary>
/// AuthController V1
/// 인증 관련 API 컨트롤러
/// </summary>
[Route("api/v1/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
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

