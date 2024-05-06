namespace SlowlySimulate.CrossCuttingConcerns.Storage
{
    public interface IDatabaseInitializer
    {
        Task SeedAsync();
        Task EnsureAdminIdentitiesAsync();
    }
}