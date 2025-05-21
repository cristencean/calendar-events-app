using Moq;
using CalendarApp.Core.Models;
using CalendarApp.Core.Models.Requests;
using CalendarApp.Core.Interfaces;
using CalendarApp.Api.Endpoints;
using FastEndpoints;

namespace CalendarApp.Tests.Api.Endpoints
    {
    public class CreateEventEndpointTests
    {
        private readonly Mock<ICalendarAppRepository> _repositoryMock;

        public CreateEventEndpointTests()
        {
            _repositoryMock = new Mock<ICalendarAppRepository>();
        }

        [Fact]
        public async Task HandleAsync_Should_Create_Event_When_Valid()
        {
            var request = new CreateEventRequestModel
            {
                Title = "New Event",
                Description = "Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());
            _repositoryMock.Setup(r => r.Add(It.IsAny<CalendarEventModel>())).Returns(Task.CompletedTask);

            var mockEndpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(request, default);
            var response = mockEndpoint.Response;

            Assert.NotNull(response);
            Assert.True(mockEndpoint.ValidationFailures.Count == 0);
            _repositoryMock.Verify(r => r.Add(It.IsAny<CalendarEventModel>()), Times.Once);
        }

        [Fact]
        public async Task HandleAsync_Should_Reject_Event_When_Validation_Fails()
        {
            var request = new CreateEventRequestModel
            {
                Title = "",
                Description = "Desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());

            var mockEndpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(request, default);

            Assert.True(mockEndpoint.ValidationFailures.Count > 0);
            Assert.Contains(mockEndpoint.ValidationFailures, v => v.ErrorMessage.Contains("Title is required"));
        }

        [Fact]
        public async Task HandleAsync_Should_Reject_When_EndDate_Before_StartDate()
        {
            var request = new CreateEventRequestModel
            {
                Title = "Bad Event",
                Description = "Desc",
                StartDate = DateTime.UtcNow.AddHours(1),
                EndDate = DateTime.UtcNow
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());

            var mockEndpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(request, default);

            Assert.Contains(mockEndpoint.ValidationFailures, v => v.ErrorMessage.Contains("End date must be after the start date"));
        }

        [Fact]
        public async Task HandleAsync_Should_Reject_When_Overlapping_Event()
        {
            var existing = new CalendarEventModel
            {
                Id = Guid.NewGuid(),
                Title = "Existing",
                Description = "Desc",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(2)
            };

            var request = new CreateEventRequestModel
            {
                Title = "New",
                Description = "Desc",
                StartDate = DateTime.UtcNow.AddMinutes(30),
                EndDate = DateTime.UtcNow.AddHours(5)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel> { existing });

            var mockEndpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object);
            await mockEndpoint.HandleAsync(request, default);

            Assert.Contains(mockEndpoint.ValidationFailures, v => v.ErrorMessage.Contains("overlaps"));
        }
    }
}