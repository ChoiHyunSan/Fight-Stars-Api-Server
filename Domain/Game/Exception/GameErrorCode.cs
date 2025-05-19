public class GameErrorCode
{
    public static readonly ErrorDetail GameUserNotFound = new("존재하지 않는 유저입니다.", 404);
    public static ErrorDetail NotEqualData = new("데이터 값이 일치하지 않습니다.", 400);
    public static ErrorDetail NotFoundProduct = new("존재하지 않는 품목입니다.", 404);

    public static ErrorDetail InsufficientCurrency = new("재화가 부족합니다.", 400);
    public static ErrorDetail AlreadyOwned = new("이미 가지고 있는 품목입니다.", 400);
}
