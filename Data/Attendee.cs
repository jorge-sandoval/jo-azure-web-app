﻿using Azure;
using Azure.Data.Tables;

namespace jo_azure_web_app.Data
{
    public class Attendee : ITableEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Industry { get; set; }
        public string ImageName { get; set; }

        public string PartitionKey { get; set; } 
        public string RowKey { get; set; }
        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }
    }
}
