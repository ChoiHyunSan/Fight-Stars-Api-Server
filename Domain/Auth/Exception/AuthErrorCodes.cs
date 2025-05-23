﻿/***************************

        AuthErrorCodes

***************************/
// Description
// : 인증 관련 에러 코드 정의 
//
// Author : ChoiHyunSan
public class AuthErrorCodes
{
    public static readonly ErrorDetail EmailAlreadyExists = new("이미 사용 중인 이메일입니다.", 409);
    public static readonly ErrorDetail UsernameAlreadyExists = new("이미 사용 중인 사용자 이름입니다.", 409);
    public static readonly ErrorDetail InvalidCredentials = new("아이디 또는 비밀번호가 잘못되었습니다.", 401);
    public static readonly ErrorDetail InvalidToken = new ("유효하지 않거나 만료된 토큰입니다.", 401);
    public static readonly ErrorDetail UserNotFound = new ("존재하지 않는 유저에 대한 접근입니다.", 404);
    public static readonly ErrorDetail RefreshTokenNotFound = new ("존재하지 않는 토큰입니다.", 404);
}

