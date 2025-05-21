namespace CalendarApp.Core.Models.Requests
{
    public class CreateEventRequestModel
    {
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Description { get; set; }
    }
}
