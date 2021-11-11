// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenClassroomIsNullAndLogItAsync()
        {
            // given
            Classroom randomClassroom = null;
            Classroom nullClassroom = randomClassroom;
            var nullClassroomException = new NullClassroomException();

            var expectedClassroomValidationException =
                new ClassroomValidationException(nullClassroomException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(nullClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async void ShouldThrowValidationExceptionOnCreateIfClassroomIsInvalidAndLogItAsync(
            string invalidText)
        {
            // given
            var invalidClassroom = new Classroom
            {
                Name = invalidText,
                Location = invalidText,
                Status = ClassroomStatus.Closed
            };

            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.Id),
                values: "Id is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.Name),
                values: "Text is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.Location),
                values: "Text is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.Status),
                values: "Value is invalid");

            invalidClassroomException.AddData(
                key: nameof(Classroom.CreatedDate),
                values: "Date is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedDate),
                values: "Date is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.CreatedBy),
                values: "Id is required");

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedBy),
                values: "Id is required");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
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
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.UpdatedBy = Guid.NewGuid();
            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedBy),
                values: $"Id is not the same as {nameof(Classroom.CreatedBy)}");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
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
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.UpdatedDate = GetRandomDateTime();
            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.UpdatedDate),
                values: $"Date is not the same as {nameof(Classroom.CreatedDate)}");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
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
            Classroom randomClassroom = CreateRandomClassroom(randomDate);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.CreatedDate = randomDate.AddMinutes(minutes);
            invalidClassroom.UpdatedDate = invalidClassroom.CreatedDate;
            var invalidClassroomException = new InvalidClassroomException();

            invalidClassroomException.AddData(
                key: nameof(Classroom.CreatedDate),
                values: $"Date is not recent");

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameValidationExceptionAs(
                    expectedClassroomValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(It.IsAny<Classroom>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenClassroomAlreadyExistsAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom alreadyExistsClassroom = randomClassroom;
            alreadyExistsClassroom.UpdatedBy = alreadyExistsClassroom.CreatedBy;
            string randomMessage = GetRandomMessage();
            string exceptionMessage = randomMessage;
            var duplicateKeyException = new DuplicateKeyException(exceptionMessage);

            var alreadyExistsClassroomException =
                new AlreadyExistsClassroomException(duplicateKeyException);

            var expectedClassroomValidationException =
                new ClassroomValidationException(alreadyExistsClassroomException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertClassroomAsync(alreadyExistsClassroom))
                    .ThrowsAsync(duplicateKeyException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(alreadyExistsClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertClassroomAsync(alreadyExistsClassroom),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
               broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
