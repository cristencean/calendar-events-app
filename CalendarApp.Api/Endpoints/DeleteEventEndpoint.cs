using CalendarApp.Application.Validators;
using CalendarApp.Core.Interfaces;
using CalendarApp.Core.Models;
using CalendarApp.Core.Models.Requests;
using FastEndpoints;

namespace CalendarApp.Api.Endpoints
{
    public class DeleteEventEndpoint : Endpoint<DeleteEventRequestModel>
    {
        private readonly ICalendarAppRepository _repository;

        public DeleteEventEndpoint(ICalendarAppRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Delete("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeleteEventRequestModel req, CancellationToken ctoken)
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

            await _repository.Delete(req.Id);
            await SendOkAsync(ctoken);
        }
    }
}