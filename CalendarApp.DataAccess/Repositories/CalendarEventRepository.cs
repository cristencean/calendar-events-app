using CalendarApp.Core.Interfaces;
using CalendarApp.Core.Models;
using CalendarApp.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace CalendarApp.DataAccess.Repositories
{
    public class CalendarEventRepository : ICalendarAppRepository
    {
        private readonly AppDbContext _context;

        public CalendarEventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CalendarEventModel?> GetById(Guid id) =>
            await _context.CalendarEvents.FindAsync(id);

        public async Task<IEnumerable<CalendarEventModel>> GetAll() =>
            await _context.CalendarEvents.ToListAsync();

        public async Task Add(CalendarEventModel eventItem)
        {
            await _context.CalendarEvents.AddAsync(eventItem);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Guid id)
        {
            var eventItem = await _context.CalendarEvents.FindAsync(id);
            if (eventItem != null)
            {
                _context.CalendarEvents.Remove(eventItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task Update(CalendarEventModel eventItem)
        {
            _context.CalendarEvents.Update(eventItem);
            await _context.SaveChangesAsync();
        }
    }
}