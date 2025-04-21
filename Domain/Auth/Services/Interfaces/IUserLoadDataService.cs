public interface IUserLoadDataService
{
    Task<UserLoadDataResponse> LoadUserDataAsync(long userId);
}