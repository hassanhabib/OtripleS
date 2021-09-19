// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.CalendarEntries;
using OtripleS.Web.Api.Models.CalendarEntries.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.CalendarEntries
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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnModifyIfCalendarEntryIsInvalidAndLogItAsync(
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
                values: new string[] { 
                    "Date is required",
                    $"Date is the same as {nameof(CalendarEntry.CreatedDate)}"
                });

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
                this.calendarEntryService.ModifyCalendarEntryAsync(invalidCalendarEntry);

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
            var invalidCalendarEntryException = new InvalidCalendarEntryException();

            invalidCalendarEntryException.AddData(
                key: nameof(CalendarEntry.UpdatedDate),
                values: $"Date is not recent");

            var expectedCalendarEntryValidationException =
                new CalendarEntryValidationException(invalidCalendarEntryException);

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
                broker.LogError(It.Is(SameValidationExceptionAs(expectedCalendarEntryValidationException))),
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

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDateTime);
            CalendarEntry someCalendarEntry = randomCalendarEntry;
            someCalendarEntry.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateException = new DbUpdateException();

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(databaseUpdateException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id))
                    .ThrowsAsync(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(someCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryDependencyException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnModifyIfDbUpdateConcurrencyExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDateTime);
            CalendarEntry someCalendarEntry = randomCalendarEntry;
            someCalendarEntry.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var databaseUpdateConcurrencyException = new DbUpdateConcurrencyException();
            var lockedCalendarEntryException = new LockedCalendarEntryException(databaseUpdateConcurrencyException);

            var expectedCalendarEntryDependencyException =
                new CalendarEntryDependencyException(lockedCalendarEntryException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id))
                    .ThrowsAsync(databaseUpdateConcurrencyException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(someCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryDependencyException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryDependencyException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnModifyIfServiceExceptionOccursAndLogItAsync()
        {
            // given
            int randomNegativeNumber = GetNegativeRandomNumber();
            DateTimeOffset randomDateTime = GetRandomDateTime();
            CalendarEntry randomCalendarEntry = CreateRandomCalendarEntry(randomDateTime);
            CalendarEntry someCalendarEntry = randomCalendarEntry;
            someCalendarEntry.CreatedDate = randomDateTime.AddMinutes(randomNegativeNumber);
            var serviceException = new Exception();

            var expectedCalendarEntryServiceException =
                new CalendarEntryServiceException(serviceException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id))
                    .ThrowsAsync(serviceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<CalendarEntry> modifyCalendarEntryTask =
                this.calendarEntryService.ModifyCalendarEntryAsync(someCalendarEntry);

            // then
            await Assert.ThrowsAsync<CalendarEntryServiceException>(() =>
                modifyCalendarEntryTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectCalendarEntryByIdAsync(someCalendarEntry.Id),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedCalendarEntryServiceException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
