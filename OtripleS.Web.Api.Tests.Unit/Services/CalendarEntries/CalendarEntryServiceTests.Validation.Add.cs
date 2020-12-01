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

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryIsNullAndLogItAsync()
        {
            // given
            CalendarEntry randomCalendarEntry = null;
            CalendarEntry nullCalendarEntry = randomCalendarEntry;

            var nullCalendarEntryException = new NullCalendarEntryException();

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(nullCalendarEntryException);

            // when
            ValueTask<CalendarEntry> registerCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(nullCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                registerCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
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
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.Id = Guid.Empty;

            var invalidCalendarEntryException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.Id),
                parameterValue: inputCalendarEntry.Id);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            // when
            ValueTask<CalendarEntry> addCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                addCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.CreatedBy = default;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.CreatedBy),
                parameterValue: inputCalendarEntry.CreatedBy);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> addCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                addCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.CreatedDate = default;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.CreatedDate),
                parameterValue: inputCalendarEntry.CreatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedBy = default;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedBy),
                parameterValue: inputCalendarEntry.UpdatedBy);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedDate = default;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedDate),
                parameterValue: inputCalendarEntry.UpdatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedBy = Guid.NewGuid();

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedBy),
                parameterValue: inputCalendarEntry.UpdatedBy);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedBy = randomCalendarEntry.CreatedBy;
            inputCalendarEntry.UpdatedDate = GetRandomDateTime();

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedDate),
                parameterValue: inputCalendarEntry.UpdatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedBy = inputCalendarEntry.CreatedBy;
            inputCalendarEntry.CreatedDate = dateTime.AddMinutes(minutes);
            inputCalendarEntry.UpdatedDate = inputCalendarEntry.CreatedDate;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.CreatedDate),
                parameterValue: inputCalendarEntry.CreatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
