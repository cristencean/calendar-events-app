using CalendarApp.Core.Models;
using FluentValidation;

namespace CalendarApp.Application.Validators
{
    public class EventIdValidator : AbstractValidator<CalendarEventModel>
    {
        public EventIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Id is required")
                .Must(BeAValidGuid).WithMessage("Id must be a valid Guid");
        }
        private bool BeAValidGuid(Guid id)
        {
            return id != Guid.Empty;
        }
    }
}