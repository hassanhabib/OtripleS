// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
	public partial class CalendarServiceTests
	{
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarIsNullAndLogItAsync()
        {
            // given
            Calendar randomCalendar = null;
            Calendar nullCalendar = randomCalendar;

            var nullCalendarException = new NullCalendarException();

            var expectedCalendarValidationException =
                new CalendarValidationException(nullCalendarException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(nullCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(It.IsAny<Calendar>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.Id = default;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.Id),
                parameterValue: inputCalendar.Id);

            var expectedCalendarValidationException =
                new CalendarValidationException(invalidCalendarInputException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(It.IsAny<Calendar>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenLabelIsInvalidAndLogItAsync(
            string invalidCalendarLabel)
        {
            // given
            DateTimeOffset datetime = DateTimeOffset.UtcNow;
            Calendar randomCalendar = CreateRandomCalendar(dates: datetime);
            Calendar invalidCalendar = randomCalendar;
            invalidCalendar.Label = invalidCalendarLabel;

            var invalidCalendarException = new InvalidCalendarInputException(
               parameterName: nameof(Calendar.Label),
               parameterValue: invalidCalendar.Label);

            var expectedCalendarValidationException =
                new CalendarValidationException(invalidCalendarException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(invalidCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(It.IsAny<Calendar>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.CreatedBy = default;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.CreatedBy),
                parameterValue: inputCalendar.CreatedBy);

            var expectedCalendarValidationException =
                new CalendarValidationException(invalidCalendarInputException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(It.IsAny<Calendar>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
