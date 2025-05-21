using FastEndpoints;
using CalendarApp.Core.Models.Requests;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;
using CalendarApp.Application.Validators;

namespace CalendarApp.Api.Endpoints
{
    public class GetEventByIdEndpoint : Endpoint<GetEventByIdRequestModel, CalendarEventModel>
    {
        private readonly ICalendarAppRepository _repository;

        public GetEventByIdEndpoint(ICalendarAppRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetEventByIdRequestModel req, CancellationToken ctoken)
        {
            var eventIdValidator = new EventIdValidator();
            var validationEventIdResult = await eventIdValidator.ValidateAsync(new CalendarEventModel()
            {
                Id = req.Id
            });

            if (!validationEventIdResult.IsValid)
            {
                foreach (var failure in validationEventIdResult.Errors)
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

            await SendAsync(calendarEvent, cancellation: ctoken);
        }
    }
}