using CalendarApp.Core.Models;

namespace CalendarApp.Core.Interfaces
{
    public interface ICalendarAppRepository
    {
        Task<IEnumerable<CalendarEventModel>> GetAll();
        Task<CalendarEventModel?> GetById(Guid id);
        Task Add(CalendarEventModel eventItem);
        Task Update(CalendarEventModel eventItem);
        Task Delete(Guid id);
    }
}