using Moq;
using CalendarApp.Api.Endpoints;
using CalendarApp.Core.Interfaces;
using CalendarApp.Core.Models;
using CalendarApp.Core.Models.Requests;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using CalendarApp.Application.Validators;
using FluentValidation.Results;

namespace CalendarApp.Tests.Api.Endpoints
{
    public class DeleteEventEndpointTests
    {
        private readonly Mock<ICalendarAppRepository> _repositoryMock;
        private readonly EventIdValidator _eventIdValidator;

        public DeleteEventEndpointTests()
        {
            _repositoryMock = new Mock<ICalendarAppRepository>();
            _eventIdValidator = new EventIdValidator();
        }

        [Fact]
        public async Task HandleAsync_Should_Delete_Event_When_Valid()
        {
            var request = new DeleteEventRequestModel { Id = Guid.NewGuid() };
            var existingEvent = new CalendarEventModel { Id = request.Id };

            _repositoryMock.Setup(r => r.GetById(request.Id)).ReturnsAsync(existingEvent);
            _repositoryMock.Setup(r => r.Delete(request.Id)).Returns(Task.CompletedTask);

            var endpoint = Factory.Create<DeleteEventEndpoint>(_repositoryMock.Object, _eventIdValidator);
            await endpoint.HandleAsync(request, CancellationToken.None);

            _repositoryMock.Verify(r => r.Delete(request.Id), Times.Once);
            Assert.True(endpoint.HttpContext.Response.StatusCode == StatusCodes.Status200OK);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_400_When_Id_Is_Invalid()
        {
            var request = new DeleteEventRequestModel { Id = Guid.Empty };
            var validationFailures = new[] { new ValidationFailure("Id", "Id is required") };

            var endpoint = Factory.Create<DeleteEventEndpoint>(_repositoryMock.Object, _eventIdValidator);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("Id is required"));
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
            Assert.True(endpoint.HttpContext.Response.StatusCode == StatusCodes.Status400BadRequest);
        }

        [Fact]
        public async Task HandleAsync_Should_Return_404_When_Event_Not_Found()
        {
            var request = new DeleteEventRequestModel { Id = Guid.NewGuid() };
            _repositoryMock.Setup(r => r.GetById(request.Id)).ReturnsAsync((CalendarEventModel?)null);

            var endpoint = Factory.Create<DeleteEventEndpoint>(_repositoryMock.Object, _eventIdValidator);
            await endpoint.HandleAsync(request, CancellationToken.None);

            Assert.True(endpoint.HttpContext.Response.StatusCode == StatusCodes.Status404NotFound);
            _repositoryMock.Verify(r => r.Delete(It.IsAny<Guid>()), Times.Never);
        }
    }
}
