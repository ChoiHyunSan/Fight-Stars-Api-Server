public interface IGameUserInitializer
{
    Task InitializeNewUserAsync(long accountId, string nickname);
}