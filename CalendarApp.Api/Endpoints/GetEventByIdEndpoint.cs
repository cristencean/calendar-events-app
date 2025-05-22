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
        private readonly EventIdValidator _eventIdValidator;

        public GetEventByIdEndpoint(ICalendarAppRepository repository, EventIdValidator eventIdValidator)
        {
            _repository = repository;
            _eventIdValidator = eventIdValidator;
        }

        public override void Configure()
        {
            Get("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(GetEventByIdRequestModel req, CancellationToken ctoken)
        {
            try
            {
                var validationResult = await _eventIdValidator.ValidateAsync(new CalendarEventModel()
                {
                    Id = req.Id
                });

                if (!validationResult.IsValid)
                {
                    foreach (var failure in validationResult.Errors)
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
            catch (Exception)
            {
                await SendErrorsAsync(500);
            }
        }
    }
}