using CalendarApp.Core.Models;
using FluentValidation;

namespace CalendarApp.Application.Validators
{
    public class EventModelValidator : AbstractValidator<CalendarEventModel>
    {
        private readonly IEnumerable<CalendarEventModel> _existingEvents;
        public EventModelValidator(IEnumerable<CalendarEventModel> existingEvents)
        {
            _existingEvents = existingEvents;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required")
                .MaximumLength(250).WithMessage("Title must be less than 250 characters");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required")
                .Must(BeAValidDate).WithMessage("Start date must be a valid date");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required")
                .Must(BeAValidDate).WithMessage("End date must be a valid date")
                .GreaterThan(x => x.StartDate).WithMessage("End date must be after the start date");

            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description must be less than 2000 characters");

            RuleFor(x => x)
                .Must(NotOverlapWithOtherEvents)
                .WithMessage("This event overlaps with an existing event");
        }

        private bool BeAValidDate(DateTime date)
        {
            return date != default;
        }

        private bool NotOverlapWithOtherEvents(CalendarEventModel newEvent)
        {
            return !_existingEvents.Any(existing =>
                existing.Id != newEvent.Id &&
                newEvent.StartDate < existing.EndDate &&
                newEvent.EndDate > existing.StartDate
            );
        }
    }
}