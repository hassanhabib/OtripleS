// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.AttendanceServiceTests
{
	public partial class AttendanceServiceTests
	{
		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenAttendanceIsNullAndLogItAsync()
		{
			//given
			Attendance invalidAttendance = null;
			var nullAttendanceException = new NullAttendanceException();

			var expectedAttendanceValidationException =
				new AttendanceValidationException(nullAttendanceException);

			//when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(invalidAttendance);

			//then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenAttendanceIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidAttendanceId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance invalidAttendance = randomAttendance;
			invalidAttendance.Id = invalidAttendanceId;

			var invalidAttendanceException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.Id),
				parameterValue: invalidAttendance.Id);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceException);

			//when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(invalidAttendance);

			//then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;
			inputAttendance.CreatedBy = default;

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.CreatedBy),
				parameterValue: inputAttendance.CreatedBy);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;
			inputAttendance.CreatedDate = default;

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.CreatedDate),
				parameterValue: inputAttendance.CreatedDate);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;
			inputAttendance.UpdatedBy = default;

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.UpdatedBy),
				parameterValue: inputAttendance.UpdatedBy);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;
			inputAttendance.UpdatedDate = default;

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.UpdatedDate),
				parameterValue: inputAttendance.UpdatedDate);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.UpdatedDate),
				parameterValue: inputAttendance.UpdatedDate);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
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
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance inputAttendance = randomAttendance;
			inputAttendance.UpdatedBy = inputAttendance.CreatedBy;
			inputAttendance.UpdatedDate = dateTime.AddMinutes(minutes);

			var invalidAttendanceInputException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.UpdatedDate),
				parameterValue: inputAttendance.UpdatedDate);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(invalidAttendanceInputException);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(dateTime);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(inputAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(It.IsAny<Guid>()),
					Times.Never);

			this.dateTimeBrokerMock.VerifyNoOtherCalls();
			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
		}

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyIfAttendanceDoesntExistAndLogItAsync()
		{
			// given
			int randomNegativeMinutes = GetNegativeRandomNumber();
			DateTimeOffset dateTime = GetRandomDateTime();
			Attendance randomAttendance = CreateRandomAttendance(dateTime);
			Attendance nonExistentAttendance = randomAttendance;
			nonExistentAttendance.CreatedDate = dateTime.AddMinutes(randomNegativeMinutes);
			Attendance noAttendance = null;
			var notFoundAttendanceException = new NotFoundAttendanceException(nonExistentAttendance.Id);

			var expectedAttendanceValidationException =
				new AttendanceValidationException(notFoundAttendanceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAttendanceByIdAsync(nonExistentAttendance.Id))
					.ReturnsAsync(noAttendance);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(dateTime);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(nonExistentAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(nonExistentAttendance.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
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
			Attendance randomAttendance = CreateRandomAttendance(randomDate);
			Attendance invalidAttendance = randomAttendance;
			invalidAttendance.UpdatedDate = randomDate;
			Attendance storageAttendance = randomAttendance.DeepClone();
			Guid attendanceId = invalidAttendance.Id;
			invalidAttendance.CreatedDate = storageAttendance.CreatedDate.AddMinutes(randomNumber);

			var invalidAttendanceException = new InvalidAttendanceInputException(
				parameterName: nameof(Attendance.CreatedDate),
				parameterValue: invalidAttendance.CreatedDate);

			var expectedAttendanceValidationException =
			  new AttendanceValidationException(invalidAttendanceException);

			this.storageBrokerMock.Setup(broker =>
				broker.SelectAttendanceByIdAsync(attendanceId))
					.ReturnsAsync(storageAttendance);

			this.dateTimeBrokerMock.Setup(broker =>
				broker.GetCurrentDateTime())
					.Returns(randomDate);

			// when
			ValueTask<Attendance> modifyAttendanceTask =
				this.attendanceService.ModifyAttendanceAsync(invalidAttendance);

			// then
			await Assert.ThrowsAsync<AttendanceValidationException>(() =>
				modifyAttendanceTask.AsTask());

			this.dateTimeBrokerMock.Verify(broker =>
				broker.GetCurrentDateTime(),
					Times.Once);

			this.storageBrokerMock.Verify(broker =>
				broker.SelectAttendanceByIdAsync(invalidAttendance.Id),
					Times.Once);

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedAttendanceValidationException))),
					Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}
	}
}
