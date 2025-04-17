using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

/***************************

       LoginRequest

***************************/
// Description : 로그인 요청 DTO
// Author : ChoiHyunSan

public class LoginRequest
{
    [Required]
    [MaxLength(20)]
    public string Username { get; set; }

    [Required]
    [MinLength(8)]
    [MaxLength(20)]
    public string Password { get; set; }
}

/***************************

       LoginResponse

***************************/
// Description : 로그인 응답 DTO
// Author : ChoiHyunSan
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public int UserId { get; set; } = -1;
    public string UserName { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}