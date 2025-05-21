using CalendarApp.Application.Validators;
using CalendarApp.Core.Models;

namespace CalendarApp.Tests.Application.Validators
{
    public class EventIdValidatorTests
    {
        private readonly EventIdValidator _validator;

        public EventIdValidatorTests()
        {
            _validator = new EventIdValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty_Guid()
        {
            var eventModel = new CalendarEventModel
            {
                Id = Guid.Empty
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id" && e.ErrorMessage == "Id must be a valid Guid");
        }

        [Fact]
        public void Should_Have_Error_When_Id_Is_Empty()
        {
            var eventModel = new CalendarEventModel
            {
                Id = Guid.Empty
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Id" && e.ErrorMessage == "Id is required");
        }

        [Fact]
        public void Should_Be_Valid_When_Id_Is_Valid_Guid()
        {
            var eventModel = new CalendarEventModel
            {
                Id = Guid.NewGuid()
            };

            var result = _validator.Validate(eventModel);

            Assert.True(result.IsValid);
        }
    }
}