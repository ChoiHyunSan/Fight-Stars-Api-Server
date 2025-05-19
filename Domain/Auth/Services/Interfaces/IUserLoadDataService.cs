public interface IUserLoadDataService
{
    Task<CreateGameResultResponse> CreateGameResultAsync(CreateGameResultRequest request);
    Task<UserLoadDataResponse> LoadUserDataAsync(long userId);
}