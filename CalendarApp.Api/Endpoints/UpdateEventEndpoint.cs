using FastEndpoints;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;
using CalendarApp.Application.Validators;

namespace CalendarApp.Api.Endpoints
{
    public class UpdateEventEndpoint : Endpoint<CalendarEventModel>
    {
        private readonly ICalendarAppRepository _repository;
        private readonly EventModelValidator _eventModelValidator;
        private readonly EventIdValidator _eventIdValidator;

        public UpdateEventEndpoint(ICalendarAppRepository repository, 
            EventModelValidator eventModelValidator,
            EventIdValidator eventIdValidator)
        {
            _repository = repository;
            _eventModelValidator = eventModelValidator;
            _eventIdValidator = eventIdValidator;
        }

        public override void Configure()
        {
            Put("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CalendarEventModel req, CancellationToken ctoken)
        {
            try { 
                var validationResults = await Task.WhenAll(
                    _eventModelValidator.ValidateAsync(req, ctoken),
                    _eventIdValidator.ValidateAsync(req, ctoken)
                );
                var allFailures = validationResults.SelectMany(r => r.Errors);
                if (allFailures.Any())
                {
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
            catch (Exception)
            {
                await SendErrorsAsync(500);
            }
        }
    }
}