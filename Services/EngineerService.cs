using jo_azure_web_app.Data;
using Microsoft.Azure.Cosmos;

namespace jo_azure_web_app.Services
{
    public class EngineerService : IEngineerService
    {

        private const string _containerName = "Engineers";

        private readonly CosmosClient _cosmosClient;
        private readonly Container _container;

        public EngineerService(IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("AzureCosmoDBConnection");
            var databaseName = configuration["CosmosDB:DatabaseName"];

            _cosmosClient = new CosmosClient(connectionString);
            _container = _cosmosClient.GetContainer(databaseName, _containerName);
        }

        public async Task AddEnginner(Engineer enginner)
        {
            try
            {
                enginner.id = Guid.NewGuid();
                var response = await _container.CreateItemAsync(enginner, new PartitionKey(enginner.id.ToString()));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task UpdateEnginner(Engineer enginner)
        {
            try
            {
                var response = await _container.UpsertItemAsync(enginner, new PartitionKey(enginner.id.ToString()));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public async Task DeleteEnginner(string? id, string? partitionKey)
        {
            try
            {
                var response = await _container.DeleteItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }
        }

        public async Task<Engineer?> GetEnginnerById(string? id, string? partitionKey)
        {
            Engineer enginner = null;
            try
            {
                var response = await _container.ReadItemAsync<Engineer>(id, new PartitionKey(partitionKey));
                enginner = response.Resource;
                Console.WriteLine(response.StatusCode);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());

            }
            return enginner;
        }

        public async Task<List<Engineer>> GetEngineersAsync()
        {
            var engineers = new List<Engineer>();
            try
            {
                var query = "SELECT * FROM c";
                var queryDefinition = new QueryDefinition(query);
                var queryResultSetIterator = _container.GetItemQueryIterator<Engineer>(queryDefinition);

                while (queryResultSetIterator.HasMoreResults)
                {
                    var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                    foreach (var engineer in currentResultSet)
                    {
                        engineers.Add(engineer);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message.ToString());
            }

            return engineers;
        }
    }
}
