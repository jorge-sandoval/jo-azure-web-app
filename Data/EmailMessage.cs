namespace jo_azure_web_app.Data
{
    public class EmailMessage
    {
        public string EmailAddress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
