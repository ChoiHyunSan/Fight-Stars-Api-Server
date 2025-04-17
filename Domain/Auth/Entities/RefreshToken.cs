using System.ComponentModel.DataAnnotations;

/***************************

         RefreshToken

***************************/
// Description
// : 유저가 로그인 시, AccessToken과 함께 발급되는 RefreshToken을 저장하는 엔티티입니다.
//   AccessToken이 만료된 경우, RefreshToken을 사용하여 새로운 AccessToken을 발급받을 수 있습니다.
//   RefreshToken은 만료 시각이 있으며, 만료된 경우에는 더 이상 사용이 불가능합니다.
//   RefreshToken은 1회성으로 사용되며, 사용 후에는 무효화됩니다.
//   RefreshToken은 로그아웃 시에도 무효화됩니다.
//   RefreshToken을 이용해 AccessToken을 재발급받을 수 있으며, 이 경우 RefreshToken 또한 새로 발급됩니다. 
//
// Author : ChoiHyunSan
public class RefreshToken
{
    [Key]
    public int Id { get; set; } // PK

    [Required]
    [MaxLength(100)]
    public string Token { get; set; } = string.Empty;   // 실제 토큰 문자열

    [Required]
    public int UserId { get; set; } = -1; // 유저 식별 ID

    [Required]
    public DateTime ExpiresAt { get; set; }  // 토큰 만료 시각

    public bool IsRevoked { get; set; } = false; // 로그아웃이나 기타 이유로 무효화 여부

    public bool IsUsed { get; set; } = false; // 이미 사용되었는지 여부 (1회성 처리)

    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow; // 생성 시간

    public void Verify()
    {
        if (IsUsed || IsRevoked || ExpiresAt < DateTime.UtcNow)
        {
            throw new ApiException(AuthErrorCodes.InvalidToken);
        }
    }
}