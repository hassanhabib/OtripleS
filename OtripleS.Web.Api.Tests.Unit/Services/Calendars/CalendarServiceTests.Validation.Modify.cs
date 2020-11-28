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
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarIsNullAndLogItAsync()
		{
			//given
			Calendar invalidCalendar = null;
			var nullCalendarException = new NullCalendarException();

			var expectedCalendarValidationException =
				new CalendarValidationException(nullCalendarException);

			//when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			//then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidCalendarId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.Id = invalidCalendarId;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.Id),
				parameterValue: invalidCalendar.Id);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			//when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			//then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Theory]
		[InlineData(null)]
		[InlineData("")]
		[InlineData("   ")]
		public async Task ShouldThrowValidationExceptionOnModifyWhenCalendarLabelIsInvalidAndLogItAsync(
				  string invalidCalendarLabel)
		{
			// given
			Calendar randomCalendar = CreateRandomCalendar(DateTime.Now);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.Label = invalidCalendarLabel;

			var invalidCalendarInputException = new InvalidCalendarInputException(
			   parameterName: nameof(Calendar.Label),
			   parameterValue: invalidCalendar.Label);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.CreatedBy = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedBy),
				parameterValue: inputCalendar.CreatedBy);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.UpdatedBy = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedBy),
				parameterValue: inputCalendar.UpdatedBy);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.CreatedDate = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedDate),
				parameterValue: inputCalendar.CreatedDate);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.UpdatedDate = default;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedDate),
				parameterValue: inputCalendar.UpdatedDate);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedDate),
				parameterValue: inputCalendar.UpdatedDate);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
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
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar inputCalendar = randomCalendar;
			inputCalendar.UpdatedBy = inputCalendar.CreatedBy;
			inputCalendar.UpdatedDate = dateTime.AddMinutes(minutes);

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedDate),
				parameterValue: inputCalendar.UpdatedDate);

			var expectedCalendarValidationException =
				new CalendarValidationException(invalidCalendarInputException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(dateTime);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(inputCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyIfCalendarDoesntExistAndLogItAsync()
		{
			// given
			int randomNegativeMinutes = GetNegativeRandomNumber();
			DateTimeOffset dateTime = GetRandomDateTime();
			Calendar randomCalendar = CreateRandomCalendar(dateTime);
			Calendar nonExistentCalendar = randomCalendar;
			nonExistentCalendar.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
			Calendar noCalendar = null;
			var notFoundCalendarException = new NotFoundCalendarException(nonExistentCalendar.Id);

			var expectedCalendarValidationException =
				new CalendarValidationException(notFoundCalendarException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(nonExistentCalendar.Id))
					.ReturnsAsync(noCalendar);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(dateTime);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(nonExistentCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(nonExistentCalendar.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
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
			Calendar randomCalendar = CreateRandomCalendar(randomDate);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.UpdatedDate = randomDate;
			Calendar storageCalendar = randomCalendar.DeepClone();
			Guid calendarId = invalidCalendar.Id;
			invalidCalendar.CreatedDate = storageCalendar.CreatedDate.AddMinutes(randomNumber);

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedDate),
				parameterValue: invalidCalendar.CreatedDate);

			var expectedCalendarValidationException =
			  new CalendarValidationException(invalidCalendarInputException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(calendarId))
					.ReturnsAsync(storageCalendar);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(randomDate);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(invalidCalendar.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
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
			Calendar randomCalendar = CreateRandomCalendar(randomDate);
			randomCalendar.CreatedDate = randomCalendar.CreatedDate.AddMinutes(minutesInThePast);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.UpdatedDate = randomDate;
			Calendar storageCalendar = randomCalendar.DeepClone();
			Guid calendarId = invalidCalendar.Id;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.UpdatedDate),
				parameterValue: invalidCalendar.UpdatedDate);

			var expectedCalendarValidationException =
			  new CalendarValidationException(invalidCalendarInputException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(calendarId))
					.ReturnsAsync(storageCalendar);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(randomDate);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(invalidCalendar.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
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
			Calendar randomCalendar = CreateRandomCalendar(randomDate);
			Calendar invalidCalendar = randomCalendar;
			invalidCalendar.CreatedDate = randomDate.AddMinutes(randomNegativeMinutes);
			Calendar storageCalendar = randomCalendar.DeepClone();
			Guid calendarId = invalidCalendar.Id;
			invalidCalendar.CreatedBy = invalidCreatedBy;

			var invalidCalendarInputException = new InvalidCalendarInputException(
				parameterName: nameof(Calendar.CreatedBy),
				parameterValue: invalidCalendar.CreatedBy);

			var expectedCalendarValidationException =
			  new CalendarValidationException(invalidCalendarInputException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectCalendarByIdAsync(calendarId))
					.ReturnsAsync(storageCalendar);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(randomDate);

			// when
			ValueTask<Calendar> modifyCalendarTask =
				this.calendarService.ModifyCalendarAsync(invalidCalendar);

			// then
			await Assert.ThrowsAsync<CalendarValidationException>(() =>
				modifyCalendarTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectCalendarByIdAsync(invalidCalendar.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedCalendarValidationException))),
					Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
