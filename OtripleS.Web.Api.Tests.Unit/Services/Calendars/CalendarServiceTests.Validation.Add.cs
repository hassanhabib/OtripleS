// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.CreatedDate = default;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.CreatedDate),
                parameterValue: inputCalendar.CreatedDate);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = default;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.UpdatedBy),
                parameterValue: inputCalendar.UpdatedBy);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedDate = default;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.UpdatedDate),
                parameterValue: inputCalendar.UpdatedDate);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = Guid.NewGuid();

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.UpdatedBy),
                parameterValue: inputCalendar.UpdatedBy);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnAddWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = randomCalendar.CreatedBy;
            inputCalendar.UpdatedDate = GetRandomDateTime();

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.UpdatedDate),
                parameterValue: inputCalendar.UpdatedDate);

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
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnAddWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar inputCalendar = randomCalendar;
            inputCalendar.UpdatedBy = inputCalendar.CreatedBy;
            inputCalendar.CreatedDate = dateTime.AddMinutes(minutes);
            inputCalendar.UpdatedDate = inputCalendar.CreatedDate;

            var invalidCalendarInputException = new InvalidCalendarInputException(
                parameterName: nameof(Calendar.CreatedDate),
                parameterValue: inputCalendar.CreatedDate);

            var expectedCalendarValidationException =
                new CalendarValidationException(invalidCalendarInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(inputCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
        public async void ShouldThrowValidationExceptionOnAddWhenCalendarAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Calendar randomCalendar = CreateRandomCalendar(dateTime);
            Calendar alreadyExistsCalendar = randomCalendar;
            alreadyExistsCalendar.UpdatedBy = alreadyExistsCalendar.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsCalendarException =
                new AlreadyExistsCalendarException(duplicateKeyException);

            var expectedCalendarValidationException =
                new CalendarValidationException(alreadyExistsCalendarException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertCalendarAsync(alreadyExistsCalendar))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Calendar> registerCalendarTask =
                this.calendarService.AddCalendarAsync(alreadyExistsCalendar);

            // then
            await Assert.ThrowsAsync<CalendarValidationException>(() =>
                registerCalendarTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertCalendarAsync(alreadyExistsCalendar),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
