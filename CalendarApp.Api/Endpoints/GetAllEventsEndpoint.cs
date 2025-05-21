using FastEndpoints;
using CalendarApp.Core.Models;
using CalendarApp.Core.Interfaces;

namespace CalendarApp.Api.Endpoints
{
    public class GetAllEventsEndpoint : EndpointWithoutRequest<List<CalendarEventModel>>
    {
        private readonly ICalendarAppRepository _repository;

        public GetAllEventsEndpoint(ICalendarAppRepository repository)
        {
            _repository = repository;
        }

        public override void Configure()
        {
            Get("/calendar-events");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ctoken)
        {
            var events = await _repository.GetAll();
            var eventsList = events?.ToList() ?? new List<CalendarEventModel>();
            await SendAsync(eventsList, cancellation: ctoken);
        }
    }
}