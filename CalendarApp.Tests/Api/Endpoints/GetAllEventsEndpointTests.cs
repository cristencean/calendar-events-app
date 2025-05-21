using FastEndpoints;
using Moq;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;
using CalendarApp.Api.Endpoints;

namespace CalendarApp.Tests.Api.Endpoints
{
    public class GetAllEventsEndpointTests
    {
        private readonly Mock<ICalendarAppRepository> _repositoryMock;

        public GetAllEventsEndpointTests()
        {
            _repositoryMock = new Mock<ICalendarAppRepository>();
        }

        [Fact]
        public async Task HandleAsync_CallsGetAllAndSendsNonNullEvents()
        {
            var expectedEvents = new List<CalendarEventModel>
            {
                new CalendarEventModel { Id = new Guid(), Title = "Event 1" },
                new CalendarEventModel { Id = new Guid(), Title = "Event 2" }
            };
            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(expectedEvents.AsEnumerable());

            var mockEndpoint = Factory.Create<GetAllEventsEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(default);
            var response = mockEndpoint.Response;

            _repositoryMock.Verify(r => r.GetAll(), Times.Once());
            Assert.Contains(response, e => e.Title == expectedEvents[0].Title);
            Assert.Contains(response, e => e.Title == expectedEvents[1].Title);
        }

        [Fact]
        public async Task HandleAsync_HandlesEmptyEventsList()
        {
            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>().AsEnumerable());

            var mockEndpoint = Factory.Create<GetAllEventsEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(default);

            _repositoryMock.Verify(r => r.GetAll(), Times.Once());
        }
    }
}