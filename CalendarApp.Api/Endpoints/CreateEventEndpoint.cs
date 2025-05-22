using FastEndpoints;
using CalendarApp.Core.Models.Requests;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;
using CalendarApp.Application.Validators;

namespace CalendarApp.Api.Endpoints
{
    public class CreateEventEndpoint : Endpoint<CreateEventRequestModel, CalendarEventModel>
    {
        private readonly ICalendarAppRepository _repository; 
        private readonly EventModelValidator _eventModelValidator;

        public CreateEventEndpoint(ICalendarAppRepository repository, EventModelValidator eventModelValidator)
        {
            _repository = repository;
            _eventModelValidator = eventModelValidator;
        }

        public override void Configure()
        {
            Post("/calendar-events");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateEventRequestModel req, CancellationToken ctoken)
        {
            try
            {
                var newCalendarEvent = new CalendarEventModel
                {
                    Id = Guid.NewGuid(),
                    Title = req.Title,
                    StartDate = req.StartDate,
                    EndDate = req.EndDate,
                    Description = req.Description
                };

                var validationResult = await _eventModelValidator.ValidateAsync(newCalendarEvent);
                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
                    {
                        AddError(failure.ErrorMessage);
                    }

                    await SendErrorsAsync(400);
                    return;
                }

                await _repository.Add(newCalendarEvent);

                await SendAsync(newCalendarEvent, cancellation: ctoken);
            }
            catch (Exception)
            {
                await SendErrorsAsync(500);
            }
        }
    }
}