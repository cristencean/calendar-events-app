using CalendarApp.Core.Models;
using CalendarApp.DataAccess.Data;
using CalendarApp.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;


namespace CalendarApp.Tests.DataAccess.Repositories
{
    public class CalendarEventRepositoryTests
    {
        private AppDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new AppDbContext(options);
        }

        [Fact]
        public async Task Add_Should_Insert_Event()
        {
            var context = GetInMemoryDbContext();
            var repo = new CalendarEventRepository(context);

            var newEvent = new CalendarEventModel
            {
                Id = Guid.NewGuid(),
                Title = "Test mock Event",
                Description = "Test mock Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddDays(1)
            };

            await repo.Add(newEvent);

            var inserted = await context.CalendarEvents.FindAsync(newEvent.Id);
            Assert.NotNull(inserted);
            Assert.Equal("Test mock Event", inserted!.Title);
        }

        [Fact]
        public async Task GetAll_Should_Return_All_Events()
        {
            var context = GetInMemoryDbContext();
            context.CalendarEvents.AddRange(new[]
            {
            new CalendarEventModel { Id = Guid.NewGuid(), Title = "Event ab", StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddHours(15) },
            new CalendarEventModel { Id = Guid.NewGuid(), Title = "Event zz", StartDate = DateTime.UtcNow.AddDays(1), EndDate = DateTime.UtcNow.AddDays(2).AddHours(15) }
        });
            await context.SaveChangesAsync();

            var repo = new CalendarEventRepository(context);

            var result = await repo.GetAll();

            Assert.Equal(2, await context.CalendarEvents.CountAsync());
            Assert.Equal(2, ((List<CalendarEventModel>)result).Count);
        }

        [Fact]
        public async Task GetById_Should_Return_Correct_Event()
        {
            var context = GetInMemoryDbContext();
            var eventId = Guid.NewGuid();
            var eventItem = new CalendarEventModel
            {
                Id = eventId,
                Title = "Lookup mock Event",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };
            context.CalendarEvents.Add(eventItem);
            await context.SaveChangesAsync();

            var repo = new CalendarEventRepository(context);

            var result = await repo.GetById(eventId);

            Assert.NotNull(result);
            Assert.Equal("Lookup mock Event", result!.Title);
        }

        [Fact]
        public async Task Update_Should_Modify_Event()
        {
            var context = GetInMemoryDbContext();
            var eventId = Guid.NewGuid();
            var original = new CalendarEventModel
            {
                Id = eventId,
                Title = "Old Title",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };
            context.CalendarEvents.Add(original);
            await context.SaveChangesAsync();

            var repo = new CalendarEventRepository(context);

            original.Title = "New Title";
            await repo.Update(original);

            var updated = await context.CalendarEvents.FindAsync(eventId);
            Assert.Equal("New Title", updated!.Title);
        }

        [Fact]
        public async Task Delete_Should_Remove_Event_If_Exists()
        {
            var context = GetInMemoryDbContext();
            var eventId = Guid.NewGuid();
            var toDelete = new CalendarEventModel
            {
                Id = eventId,
                Title = "To Delete",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };
            context.CalendarEvents.Add(toDelete);
            await context.SaveChangesAsync();

            var repo = new CalendarEventRepository(context);

            await repo.Delete(eventId);

            var result = await context.CalendarEvents.FindAsync(eventId);
            Assert.Null(result);
        }

        [Fact]
        public async Task Delete_Should_Do_Nothing_If_Event_Does_Not_Exist()
        {
            var context = GetInMemoryDbContext();
            var repo = new CalendarEventRepository(context);

            await repo.Delete(Guid.NewGuid());

            Assert.Empty(await context.CalendarEvents.ToListAsync());
        }
    }
}