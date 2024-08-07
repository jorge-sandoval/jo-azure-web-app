using Azure;
using Azure.Data.Tables;
using jo_azure_web_app.Data;
using jo_azure_web_app.Data.Configuration;
using Microsoft.Extensions.Options;

namespace jo_azure_web_app.Services
{
    public class AttendeesService : IAttendeesService
    {
        private readonly string _tableName;
        private readonly TableClient _tableClient;

        public AttendeesService(
            IOptions<AzureStorageSettings> storageOptions,
            IOptions<ConnectionStringsSettings> connectionStringOptions
        )
        {
            _tableName = storageOptions.Value.Tables.AttendeesTableName;

            var connectionString = connectionStringOptions.Value.AzureStorageConnection;
            _tableClient = GetTableClient(connectionString);
        }

        private TableClient GetTableClient(string connectionString)
        {
            var tableServiceClient = new TableServiceClient(connectionString);

            var tableClient = tableServiceClient.GetTableClient(_tableName);
            tableClient.CreateIfNotExists();

            return tableClient;
        }

        public async Task<Attendee> GetAttendee(string industry, string id) {
            return await _tableClient.GetEntityAsync<Attendee>(industry, id);
        }

        public List<Attendee> GetAttendees() {
            Pageable<Attendee> attendees = _tableClient.Query<Attendee>();
            return attendees.ToList();
        }

        public async Task UpsertAtrendee(Attendee attendee) {
            await _tableClient.UpsertEntityAsync(attendee);
        }

        public async Task DeleteAtrendee(string industry, string id)
        {
            await _tableClient.DeleteEntityAsync(industry, id);
        }
    }
}
