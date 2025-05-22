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
        private readonly EventIdValidator _eventIdValidator;

        public DeleteEventEndpoint(ICalendarAppRepository repository, EventIdValidator eventIdValidator)
        {
            _repository = repository;
            _eventIdValidator = eventIdValidator;
        }

        public override void Configure()
        {
            Delete("/calendar-events/{id}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(DeleteEventRequestModel req, CancellationToken ctoken)
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

                await _repository.Delete(req.Id);
                await SendOkAsync(ctoken);
            }
            catch (Exception)
            {
                await SendErrorsAsync(500);
            }
        }
    }
}