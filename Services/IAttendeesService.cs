using jo_azure_web_app.Data;

namespace jo_azure_web_app.Services
{
    public interface IAttendeesService
    {
        Task DeleteAtrendee(string industry, string id);
        Task<Attendee> GetAttendee(string industry, string id);
        List<Attendee> GetAttendees();
        Task UpsertAtrendee(Attendee attendee);
    }
}