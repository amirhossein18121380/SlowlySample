namespace CrossCuttingConcerns.Storage
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
        Task EnsureAdminIdentitiesAsync();
    }
}