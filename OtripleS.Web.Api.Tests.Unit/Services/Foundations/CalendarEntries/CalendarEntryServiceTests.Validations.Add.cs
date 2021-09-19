// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenCalendarEntryLabelIsInvalidAndLogItAsync(
            string invalidCalendarEntryLabel)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.Label = invalidCalendarEntryLabel;

            var invalidCalendarEntryException = new InvalidCalendarEntryException(
               parameterName: nameof(CalendarEntry.Label),
               parameterValue: invalidCalendarEntry.Label);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnAddWhenCalendarEntryDescriptionIsInvalidAndLogItAsync(
            string invalidCalendarEntryDescription)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.Description = invalidCalendarEntryDescription;

            var invalidCalendarEntryException = new InvalidCalendarEntryException(
               parameterName: nameof(CalendarEntry.Description),
               parameterValue: invalidCalendarEntry.Description);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

            // when
            ValueTask<CalendarEntry> createCalendarEntryTask =
                this.calendarEntryService.AddCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                createCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarEntryAsync(It.IsAny<CalendarEntry>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
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
               broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
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
