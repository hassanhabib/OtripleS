// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.CalendarEntries
{
    public partial class CalendarEntryServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarEntryIsNullAndLogItAsync()
        {
            //given
            CalendarEntry invalidCalendarEntry = null;
            var nullCalendarEntryException = new NullCalendarEntryException();

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(nullCalendarEntryException);

            //when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            //then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarEntryIdIsInvalidAndLogItAsync()
        {
            //given
            Guid invalidCalendarEntryId = Guid.Empty;
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.Id = invalidCalendarEntryId;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.Id),
                parameterValue: invalidCalendarEntry.Id);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            //when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            //then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarEntryLabelIsInvalidAndLogItAsync(
                  string invalidCalendarEntryLabel)
        {
            // given
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(DateTime.Now);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.Label = invalidCalendarEntryLabel;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
               parameterName: nameof(CalendarEntry.Label),
               parameterValue: invalidCalendarEntry.Label);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedByIsInvalidAndLogItAsync()
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
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedDate),
                parameterValue: inputCalendarEntry.UpdatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry inputCalendarEntry = randomCalendarEntry;
            inputCalendarEntry.UpdatedBy = inputCalendarEntry.CreatedBy;
            inputCalendarEntry.UpdatedDate = dateTime.AddMinutes(minutes);

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedDate),
                parameterValue: inputCalendarEntry.UpdatedDate);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(inputCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfCalendarEntryDoesntExistAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            DateTimeOffset dateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(dateTime);
            CalendarEntry nonExistentCalendarEntry = randomCalendarEntry;
            nonExistentCalendarEntry.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
            CalendarEntry noCalendarEntry = null;
            
            var notFoundCalendarEntryException = 
                new NotFoundCalendarEntryException(nonExistentCalendarEntry.Id);

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(notFoundCalendarEntryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(nonExistentCalendarEntry.Id))
                    .ReturnsAsync(noCalendarEntry);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(nonExistentCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(nonExistentCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedDateNotSameAsCreateDateAndLogItAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomMinutes = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDate);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.UpdatedDate = randomDate;
            CalendarEntry storageCalendarEntry = randomCalendarEntry.DeepClone();
            Guid calendarEntryId = invalidCalendarEntry.Id;
            invalidCalendarEntry.CreatedDate = storageCalendarEntry.CreatedDate.AddMinutes(randomNumber);

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.CreatedDate),
                parameterValue: invalidCalendarEntry.CreatedDate);

            var expectedCalendarEntryValidationException =
              new CalendarEntryValidationException(invalidCalendarEntryInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(calendarEntryId))
                    .ReturnsAsync(storageCalendarEntry);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(invalidCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageUpdatedDateSameAsUpdatedDateAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            int minutesInThePast = randomNegativeMinutes;
            DateTimeOffset randomDate = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDate);
            randomCalendarEntry.CreatedDate = randomCalendarEntry.CreatedDate.AddMinutes(minutesInThePast);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.UpdatedDate = randomDate;
            CalendarEntry storageCalendarEntry = randomCalendarEntry.DeepClone();
            Guid calendarEntryId = invalidCalendarEntry.Id;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.UpdatedDate),
                parameterValue: invalidCalendarEntry.UpdatedDate);

            var expectedCalendarEntryValidationException =
              new CalendarEntryValidationException(invalidCalendarEntryInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(calendarEntryId))
                    .ReturnsAsync(storageCalendarEntry);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(invalidCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyIfStorageCreatedByNotSameAsCreatedByAndLogItAsync()
        {
            // given
            int randomNegativeMinutes = GetNegativeRandomNumber();
            Guid differentId = Guid.NewGuid();
            Guid invalidCreatedBy = differentId;
            DateTimeOffset randomDate = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDate);
            CalendarEntry invalidCalendarEntry = randomCalendarEntry;
            invalidCalendarEntry.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
            CalendarEntry storageCalendarEntry = randomCalendarEntry.DeepClone();
            Guid calendarEntryId = invalidCalendarEntry.Id;
            invalidCalendarEntry.CreatedBy = invalidCreatedBy;

            var invalidCalendarEntryInputException = new InvalidCalendarEntryException(
                parameterName: nameof(CalendarEntry.CreatedBy),
                parameterValue: invalidCalendarEntry.CreatedBy);

            var expectedCalendarEntryValidationException =
              new CalendarEntryValidationException(invalidCalendarEntryInputException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(calendarEntryId))
                    .ReturnsAsync(storageCalendarEntry);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryValidationException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(invalidCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
