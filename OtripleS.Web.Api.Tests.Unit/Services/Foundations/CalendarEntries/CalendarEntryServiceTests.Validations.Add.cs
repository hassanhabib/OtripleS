// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryIsNullAndLogItAsync()
        {
            // given
            CalendarEntry invalidCalendarEntry = null;


            var nullCalendarEntryException = new NullCalendarEntryException();

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(nullCalendarEntryException);

            // when
            ValueTask<CalendarEntry> registerCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                registerCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnCreateIfCalendarEntryIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidCalendarEntry = new CalendarEntry
            {
                Label = invalidText,
                Description = invalidText
            };

            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.Id),
                values: "Id is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.Label),
                values: "Text is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.Description),
                values: "Text is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.StartDate),
                values: "Date is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.EndDate),
                values: "Date is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.RemindAtDateTime),
                values: "Date is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.CreatedDate),
                values: "Date is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.UpdatedDate),
                values: "Date is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.CreatedBy),
                values: "Id is required");

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.UpdatedBy),
                values: "Id is required");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.UpdatedBy = Guid.NewGuid();
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.UpdatedBy),
                values: $"Id is not the same as {nameof(CalendarEntry.CreatedBy)}");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.UpdatedDate = GetRandomDateTime();
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.UpdatedDate),
                values: $"Date is not the same as {nameof(CalendarEntry.CreatedDate)}");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryValidationException))),
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
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset randomDate = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDate);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.CreatedDate = randomDate.AddMinutes(minutes);
            invalidCalendarEntry.UpdatedDate = invalidCalendarEntry.CreatedDate;
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.CreatedDate),
                values: $"Date is not recent");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenEndDateIsBeforeStartDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            int randomMinutes = GetRandomNumber();
            inputCalendarEntry.UpdatedDate = dateTime.AddMinutes(randomMinutes);
            inputCalendarEntry.StartDate = dateTime.AddMinutes(randomMinutes);
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.EndDate),
                values: $"Date is before StartDate");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedCalendarEntryValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarEntryAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry alreadyExistsCalendarEntry = randomCalendarEntry;
            alreadyExistsCalendarEntry.UpdatedBy = alreadyExistsCalendarEntry.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCalendarEntryException =
                new AlreadyExistsCalendarEntryException(duplicateKeyException);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(alreadyExistsCalendarEntryException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarEntryAsync(alreadyExistsCalendarEntry))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(alreadyExistsCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedCalendarEntryValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(alreadyExistsCalendarEntry),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
