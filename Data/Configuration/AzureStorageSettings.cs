namespace jo_azure_web_app.Data.Configuration
{
    public class AzureStorageSettings
    {
        public ContainerSettings Containers { get; set; }
        public TableSettings Tables { get; set; }
        public QueueSettings Queues { get; set; }
    }
}
