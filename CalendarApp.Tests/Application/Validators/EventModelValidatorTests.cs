using CalendarApp.Application.Validators;
using CalendarApp.Core.Models;

namespace CalendarApp.Tests.Application.Validators
{
    public class EventModelValidatorTests
    {
        private readonly EventModelValidator _validator;
        private readonly List<CalendarEventModel> _existingEvents;

        public EventModelValidatorTests()
        {
            _existingEvents = new List<CalendarEventModel>
            {
                new CalendarEventModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Some event in db",
                    StartDate = DateTime.Now,
                    EndDate = DateTime.Now.AddHours(5),
                    Description = "Some random event"
                }
            };
            _validator = new EventModelValidator(_existingEvents);
        }

        #region Title Validation

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            var eventModel = new CalendarEventModel
            {
                Title = string.Empty
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title" && e.ErrorMessage == "Title is required");
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Too_Long()
        {
            var eventModel = new CalendarEventModel
            {
                Title = new string('z', 300)
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Title" && e.ErrorMessage == "Title must be less than 250 characters");
        }

        #endregion

        #region Start Date Validation

        [Fact]
        public void Should_Have_Error_When_StartDate_Is_Default()
        {
            var eventModel = new CalendarEventModel
            {
                StartDate = default(DateTime)
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "StartDate" && e.ErrorMessage == "Start date must be a valid date");
        }

        #endregion

        #region End Date Validation

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Default()
        {
            var eventModel = new CalendarEventModel
            {
                EndDate = default(DateTime)
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "EndDate" && e.ErrorMessage == "End date must be a valid date");
        }

        [Fact]
        public void Should_Have_Error_When_EndDate_Is_Before_StartDate()
        {
            var eventModel = new CalendarEventModel
            {
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "EndDate" && e.ErrorMessage == "End date must be after the start date");
        }

        #endregion

        #region Description Validation

        [Fact]
        public void Should_Have_Error_When_Description_Is_Too_Long()
        {
            var eventModel = new CalendarEventModel
            {
                Description = new string('z', 2200)
            };

            var result = _validator.Validate(eventModel);

            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Description" && e.ErrorMessage == "Description must be less than 2000 characters");
        }

        #endregion

        [Fact]
        public void Should_Be_Valid_When_Description_Title_StartDate_EndDate_Is_Valid()
        {
            var eventModel = new CalendarEventModel
            {
                Title = "Valid Title",
                StartDate = DateTime.Now.AddDays(1),
                EndDate = DateTime.Now.AddDays(2),
                Description = "This is a valid description."
            };

            var result = _validator.Validate(eventModel);

            Assert.True(result.IsValid);
        }
    }
}
