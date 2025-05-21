using FastEndpoints;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;
using CalendarApp.Application.Validators;

namespace CalendarApp.Api.Endpoints
{
    public class UpdateEventEndpoint : Endpoint<CalendarEventModel>
    {
        private readonly ICalendarAppRepository _repository;

        public UpdateEventEndpoint(ICalendarAppRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Put("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CalendarEventModel req, CancellationToken ctoken)
        {
            var allEvents = await _repository.GetAll();

            var eventModelValidator = new EventModelValidator(allEvents);
            var validationEventModelResult = await eventModelValidator.ValidateAsync(req);

            var eventIdValidator = new EventIdValidator();
            var validationEventIdResult = await eventIdValidator.ValidateAsync(req);

            if (!validationEventModelResult.IsValid || !validationEventIdResult.IsValid)
            {
                var allFailures = validationEventModelResult.Errors
                    .Concat(validationEventIdResult.Errors);
                foreach (var failure in allFailures)
                {
                    AddError(failure.ErrorMessage);
                }

                await SendErrorsAsync(400);
                return;
            }

            var calendarEvent = await _repository.GetById(req.Id);
            if (calendarEvent is null)
            {
                await SendNotFoundAsync();
                return;
            }

            calendarEvent.Title = req.Title;
            calendarEvent.StartDate = req.StartDate;
            calendarEvent.EndDate = req.EndDate;
            calendarEvent.Description = req.Description;

            await _repository.Update(calendarEvent);

            await SendAsync(calendarEvent, cancellation: ctoken);
        }
    }
}