
namespace jo_azure_web_app.Data
{
    public interface IEngineerService
    {
        Task AddEnginner(Engineer enginner);
        Task DeleteEnginner(string? id, string? partitionKey);
        Task<List<Engineer>> GetEngineersAsync();
        Task<Engineer?> GetEnginnerById(string? id, string? partitionKey);
        Task UpdateEnginner(Engineer enginner);
    }
}