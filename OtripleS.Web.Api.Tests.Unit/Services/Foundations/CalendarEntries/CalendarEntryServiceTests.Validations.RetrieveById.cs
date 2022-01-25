// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomCalendarEntryId = default;
            Guid inputCalendarEntryId = randomCalendarEntryId;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.Id),
                parameterValue: inputCalendarEntryId);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> retrieveCalendarEntryByIdTask =
                this.calendarEntryService.RetrieveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                retrieveCalendarEntryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(
                    It.IsAny<Guid>()),
                        Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void
            ShouldThrowValidationExceptionOnRetrieveWhenStorageCalendarEntryIsNullAndLogItAsync()
        {
            // given
            Guid randomCalendarEntryId = Guid.NewGuid();
            Guid inputCalendarEntryId = randomCalendarEntryId;
            CalendarEntry invalidStorageCalendarEntry = null;

            var notFoundCalendarEntryException =
                new NotFoundCalendarEntryException(inputCalendarEntryId);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(notFoundCalendarEntryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId))
                    .ReturnsAsync(invalidStorageCalendarEntry);

            // when
            ValueTask<CalendarEntry> retrieveCalendarEntryByIdTask =
                this.calendarEntryService.RetrieveCalendarEntryByIdAsync(inputCalendarEntryId);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                retrieveCalendarEntryByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(inputCalendarEntryId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
