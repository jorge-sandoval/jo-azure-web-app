using Azure;
using Azure.Data.Tables;
using jo_azure_web_app.Data;

namespace jo_azure_web_app.Services
{
    public class AttendeesService : IAttendeesService
    {
        private const string _tableName = "Attendees";
        private readonly TableClient _tableClient;

        public AttendeesService(IConfiguration configuration)
        {
            _tableClient = GetTableClient(configuration);
        }

        private TableClient GetTableClient(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureStorageConnection");
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
