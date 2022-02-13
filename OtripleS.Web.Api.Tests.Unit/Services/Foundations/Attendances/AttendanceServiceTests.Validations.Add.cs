// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Attendances;
using OtripleS.Web.Api.Models.Attendances.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Attendances
{
    public partial class AttendanceServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttendanceIsNullAndLogItAsync()
        {
            // given
            Attendance invalidAttendance = null;
            
            var nullAttendanceException = new NullAttendanceException();

            var expectedAttendanceValidationException =
                new AttendanceValidationException(nullAttendanceException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnCreateIfAttendanceIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidStuent = new Attendance
            {
                Notes = invalidText
            };

            var invalidAttendanceException = new InvalidAttendanceException();

            invalidAttendanceException.AddData(
                key: nameof(Attendance.Id),
                values: "Id is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.StudentSemesterCourseId),
                values: "Id is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.AttendanceDate),
                values: "Date is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.Notes),
                values: "Text is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.CreatedBy),
                values: "Id is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.UpdatedBy),
                values: "Id is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.CreatedDate),
                values: "Date is required");

            invalidAttendanceException.AddData(
                key: nameof(Attendance.UpdatedDate),
                values: "Date is required");

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidStuent);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.AtLeastOnce);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async Task ShouldThrowValidationExceptionOnCreateWhenAttendanceDateIsInvalidAndLogItAsync(
            int invallidMinutes)
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: randomDateTime);
            Attendance invalidAttendance = randomAttendance;
            invalidAttendance.UpdatedBy = invalidAttendance.CreatedBy;
            invalidAttendance.UpdatedDate = invalidAttendance.CreatedDate;
            invalidAttendance.AttendanceDate = randomDateTime.AddMinutes(invallidMinutes);
            var invalidAttendanceException = new InvalidAttendanceException();

            invalidAttendanceException.AddData(
                key: nameof(Attendance.AttendanceDate),
                values: $"Date is not recent");

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.AtLeastOnce);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int invallidMinutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime: dateTime);
            Attendance inputAttendance = randomAttendance;
            inputAttendance.UpdatedBy = inputAttendance.CreatedBy;
            inputAttendance.CreatedDate = dateTime.AddMinutes(invallidMinutes);
            inputAttendance.UpdatedDate = inputAttendance.CreatedDate;
            var invalidAttendanceException = new InvalidAttendanceException();

            invalidAttendanceException.AddData(
                key: nameof(Attendance.CreatedDate),
                values: $"Date is not recent");

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.AtLeastOnce);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance inputAttendance = randomAttendance.DeepClone();
            inputAttendance.UpdatedBy = randomAttendance.CreatedBy;
            inputAttendance.UpdatedDate = GetRandomDateTime();
            var invalidAttendanceException = new InvalidAttendanceException();

            invalidAttendanceException.AddData(
                key: nameof(Attendance.UpdatedDate),
                values: $"Date is not the same as {nameof(Attendance.CreatedDate)}");

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(inputAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.AtLeastOnce);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance invalidAttendance = randomAttendance;
            invalidAttendance.UpdatedBy = Guid.NewGuid();

            var invalidAttendanceInputException = new InvalidAttendanceException();

            invalidAttendanceInputException.AddData(
                key: nameof(Attendance.UpdatedBy),
                values: $"Id is not the same as {nameof(Attendance.CreatedBy)}");

            var expectedAttendanceValidationException =
                new AttendanceValidationException(invalidAttendanceInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(invalidAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedAttendanceValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(It.IsAny<Attendance>()),
                    Times.Never);

            this.dateTimeBrokerMock.Verify(broker =>
               broker.GetCurrentDateTime(),
                   Times.AtLeastOnce);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenAttendanceAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Attendance randomAttendance = CreateRandomAttendance(dateTime);
            Attendance alreadyExistsAttendance = randomAttendance;
            alreadyExistsAttendance.UpdatedBy = alreadyExistsAttendance.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsAttendanceException =
                new AlreadyExistsAttendanceException(duplicateKeyException);

            var expectedAttendanceValidationException =
                new AttendanceValidationException(alreadyExistsAttendanceException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertAttendanceAsync(alreadyExistsAttendance))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Attendance> createAttendanceTask =
                this.attendanceService.CreateAttendanceAsync(alreadyExistsAttendance);

            // then
            await Assert.ThrowsAsync<AttendanceValidationException>(() =>
                createAttendanceTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.AtLeastOnce);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertAttendanceAsync(alreadyExistsAttendance),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(
                   expectedAttendanceValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
