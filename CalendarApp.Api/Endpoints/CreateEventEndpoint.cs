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

        public CreateEventEndpoint(ICalendarAppRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Post("/calendar-events");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CreateEventRequestModel req, CancellationToken ctoken)
        {
            var allEvents = await _repository.GetAll();
            var eventModelValidator = new EventModelValidator(allEvents);

            var newCalendarEvent = new CalendarEventModel
            {
                Id = Guid.NewGuid(),
                Title = req.Title,
                StartDate = req.StartDate,
                EndDate = req.EndDate,
                Description = req.Description
            };

            var validationEventModelResult = await eventModelValidator.ValidateAsync(newCalendarEvent);
            if (!validationEventModelResult.IsValid)
            {
                foreach (var failure in validationEventModelResult.Errors)
                {
                    AddError(failure.ErrorMessage);
                }

                await SendErrorsAsync(400);
                return;
            }

            await _repository.Add(newCalendarEvent);

            await SendAsync(newCalendarEvent, cancellation: ctoken);
        }
    }
}