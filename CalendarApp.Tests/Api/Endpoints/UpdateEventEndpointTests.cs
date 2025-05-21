using CalendarApp.Api.Endpoints;
using CalendarApp.Core.Interfaces;
using CalendarApp.Core.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CalendarApp.Tests.Api.Endpoints
{
    public class UpdateEventEndpointTests
    {
        private readonly Mock<ICalendarAppRepository> _repositoryMock;

        public UpdateEventEndpointTests()
        {
            _repositoryMock = new Mock<ICalendarAppRepository>();
        }

        [Fact]
        public async Task HandleAsync_Should_Update_Event_When_Valid()
        {
            var id = Guid.NewGuid();
            var request = new CalendarEventModel
            {
                Id = id,
                Title = "Updated",
                Description = "Updated description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            var existing = new CalendarEventModel
            {
                Id = id,
                Title = "Old",
                Description = "Old description",
                StartDate = DateTime.UtcNow.AddHours(-2),
                EndDate = DateTime.UtcNow.AddHours(-1)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel> { existing });
            _repositoryMock.Setup(r => r.GetById(id)).ReturnsAsync(existing);
            _repositoryMock.Setup(r => r.Update(It.IsAny<CalendarEventModel>())).Returns(Task.CompletedTask);

            var endpoint = Factory.Create<UpdateEventEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            _repositoryMock.Verify(r => r.Update(It.Is<CalendarEventModel>(e =>
                e.Title == request.Title &&
                e.Description == request.Description &&
                e.StartDate == request.StartDate &&
                e.EndDate == request.EndDate
            )), Times.Once);

            Assert.Equal(StatusCodes.Status200OK, endpoint.HttpContext.Response.StatusCode);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_400_When_Model_Is_Invalid()
        {
            var request = new CalendarEventModel
            {
                Id = Guid.NewGuid(),
                Title = "",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(-1)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());

            var endpoint = Factory.Create<UpdateEventEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("Title is required") || v.ErrorMessage.Contains("End date must be after the start date"));
            _repositoryMock.Verify(r => r.Update(It.IsAny<CalendarEventModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_404_When_Event_Not_Found()
        {
            var request = new CalendarEventModel
            {
                Id = Guid.NewGuid(),
                Title = "Valid Title",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());
            _repositoryMock.Setup(r => r.GetById(request.Id)).ReturnsAsync((CalendarEventModel?)null);

            var endpoint = Factory.Create<UpdateEventEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.Equal(StatusCodes.Status404NotFound, endpoint.HttpContext.Response.StatusCode);
            _repositoryMock.Verify(r => r.Update(It.IsAny<CalendarEventModel>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_400_When_Id_Is_Empty()
        {
            var request = new CalendarEventModel
            {
                Id = Guid.Empty,
                Title = "Some title",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(new List<CalendarEventModel>());

            var endpoint = Factory.Create<UpdateEventEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("Id is required"));
            _repositoryMock.Verify(r => r.Update(It.IsAny<CalendarEventModel>()), Times.Never);
        }
    }
}
