using System.ComponentModel.DataAnnotations;

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
}