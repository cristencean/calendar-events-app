using Moq;
using CalendarApp.Core.Models;
using CalendarApp.Core.Models.Requests;
using CalendarApp.Core.Interfaces;
using CalendarApp.Api.Endpoints;
using CalendarApp.Application.Validators;
using FastEndpoints;

namespace CalendarApp.Tests.Api.Endpoints;

public class CreateEventEndpointTests
{
    private readonly Mock<ICalendarAppRepository> _repositoryMock;
    private readonly EventModelValidator _eventModelValidator;

    public CreateEventEndpointTests()
    {
        _repositoryMock = new Mock<ICalendarAppRepository>();
        _eventModelValidator = new EventModelValidator(new List<CalendarEventModel>());
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

        var existingEvents = new List<CalendarEventModel>();

        _repositoryMock.Setup(r => r.GetAll()).ReturnsAsync(existingEvents);
        _repositoryMock.Setup(r => r.Add(It.IsAny<CalendarEventModel>())).Returns(Task.CompletedTask);

        var endpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object, _eventModelValidator);
        await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.NotNull(endpoint.Response);
        Assert.Empty(endpoint.ValidationFailures);
        _repositoryMock.Verify(r => r.Add(It.IsAny<CalendarEventModel>()), Times.Once);
    }

    [Fact]
    public async Task HandleAsync_Should_Reject_Event_When_Validation_Fails()
    {
        var request = new CreateEventRequestModel
        {
            Title = "", // Invalid
            Description = "Desc",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow
        };

        var endpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object, _eventModelValidator);
        await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.True(endpoint.ValidationFailures.Count > 0);
        Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("Title is required"));
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

        var endpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object, _eventModelValidator);
        await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("End date must be after the start date"));
    }

    [Fact]
    public async Task HandleAsync_Should_Reject_When_Overlapping_Event()
    {
        var existingEvent = new CalendarEventModel
        {
            Id = Guid.NewGuid(),
            Title = "Existing Event",
            Description = "Desc",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddHours(2)
        };

        var request = new CreateEventRequestModel
        {
            Title = "Overlapping Event",
            Description = "Desc",
            StartDate = DateTime.UtcNow.AddMinutes(30),
            EndDate = DateTime.UtcNow.AddHours(3)
        };

        var endpoint = Factory.Create<CreateEventEndpoint>(_repositoryMock.Object, new EventModelValidator(new List<CalendarEventModel>() { existingEvent }));
        await endpoint.HandleAsync(request, CancellationToken.None);

        Assert.Contains(endpoint.ValidationFailures, v => v.ErrorMessage.Contains("overlaps"));
    }
}
