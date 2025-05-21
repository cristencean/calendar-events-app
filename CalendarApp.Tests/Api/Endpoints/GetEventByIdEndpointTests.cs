using CalendarApp.Api.Endpoints;
using CalendarApp.Core.Interfaces;
using CalendarApp.Core.Models.Requests;
using CalendarApp.Core.Models;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Moq;

namespace CalendarApp.Tests.Api.Endpoints
{
    public class GetEventByIdEndpointTests
    {
        private readonly Mock<ICalendarAppRepository> _repositoryMock;

        public GetEventByIdEndpointTests()
        {
            _repositoryMock = new Mock<ICalendarAppRepository>();
        }

        [Fact]
        public async Task HandleAsync_Should_Return_Event_When_Valid()
        {
            var request = new GetEventByIdRequestModel { Id = Guid.NewGuid() };
            var existingEvent = new CalendarEventModel
            {
                Id = request.Id,
                Title = "Test Event",
                Description = "Description",
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddHours(1)
            };

            _repositoryMock.Setup(r => r.GetById(request.Id)).ReturnsAsync(existingEvent);

            var endpoint = Factory.Create<GetEventByIdEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            var response = endpoint.Response;
            Assert.NotNull(response);
            Assert.Equal(request.Id, response.Id);
            Assert.Equal("Test Event", response.Title);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_400_When_Id_Invalid()
        {
            var request = new GetEventByIdRequestModel { Id = Guid.Empty };

            var endpoint = Factory.Create<GetEventByIdEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("Id is required"));
            _repositoryMock.Verify(r => r.GetById(It.IsAny<Guid>()), Times.Never);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_404_When_Event_Not_Found()
        {
            var request = new GetEventByIdRequestModel { Id = Guid.NewGuid() };

            _repositoryMock.Setup(r => r.GetById(request.Id)).ReturnsAsync((CalendarEventModel?)null);

            var endpoint = Factory.Create<GetEventByIdEndpoint>(_repositoryMock.Object);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.True(endpoint.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound);
        }
    }
}
