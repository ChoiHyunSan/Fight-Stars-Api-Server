/***************************

       IUserRepository

***************************/
// Description
// : AuthUser 엔티티에 대한 CRUD 작업을 수행하는 인터페이스입니다.
//
// Author : ChoiHyunSan
public interface IUserRepository
{ 
    Task<AuthUser> CreateAsync(AuthUser user);
    Task<bool> CheckDuplicatedUsername(string username);
    Task<bool> CheckDuplicatedEmail(string email);
    Task<AuthUser> FindByUsername(string userName);
    Task<AuthUser> FindByUserId(int userId);
}
