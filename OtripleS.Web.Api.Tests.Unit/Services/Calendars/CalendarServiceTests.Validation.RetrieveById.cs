// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Calendars;
using OtripleS.Web.Api.Models.Calendars.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Calendars
{
    public partial class CalendarServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCalendarId = default;
            Guid inputCalendarId = randomCalendarId;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.Id),
                parameterValue: inputCalendarId);

            var expectedCalendarValidationException =
                new CalendarValidationException(invalidCalendarInputException);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageCalendarIsNullAndLogItAsync()
        {
            // given
            Guid randomCalendarId = Guid.NewGuid();
            Guid inputCalendarId = randomCalendarId;
            Calendar invalidStorageCalendar = null;
            var notFoundCalendarException = new NotFoundCalendarException(inputCalendarId);

            var expectedCalendarValidationException =
                new CalendarValidationException(notFoundCalendarException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId))
                    .ReturnsAsync(invalidStorageCalendar);

            // when
            ValueTask<Calendar> retrieveCalendarByIdTask =
                this.calendarService.RetrieveCalendarByIdAsync(inputCalendarId);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                retrieveCalendarByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarByIdAsync(inputCalendarId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
