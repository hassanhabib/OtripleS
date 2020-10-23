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

namespace OtripleS.Web.Api.Tests.Unit.Services.Classrooms
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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenIdIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.Id = default;

            var InvalidClassroomException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.Id),
                parameterValue: inputClassroom.Id);

            var expectedClassroomValidationException =
                new ClassroomValidationException(InvalidClassroomException);

            // when
            ValueTask<Classroom> registerClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                registerClassroomTask.AsTask());

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
        public async Task ShouldThrowValidationExceptionOnCreateWhenClassroomNameIsInvalidAndLogItAsync(
            string invalidClassroomName)
        {
            // given
            Classroom randomClassroom = CreateRandomClassroom();
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.Name = invalidClassroomName;

            var invalidClassroomInputException = new InvalidClassroomInputException(
               parameterName: nameof(Classroom.Name),
               parameterValue: invalidClassroom.Name);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();

        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.CreatedBy = default;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.CreatedBy),
                parameterValue: inputClassroom.CreatedBy);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.CreatedDate = default;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.CreatedDate),
                parameterValue: inputClassroom.CreatedDate);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = default;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.UpdatedBy),
                parameterValue: inputClassroom.UpdatedBy);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedDate = default;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.UpdatedDate),
                parameterValue: inputClassroom.UpdatedDate);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedByIsNotSameToCreatedByAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = Guid.NewGuid();

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.UpdatedBy),
                parameterValue: inputClassroom.UpdatedBy);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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

        [Fact]
        public async void ShouldThrowValidationExceptionOnCreateWhenUpdatedDateIsNotSameToCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = randomClassroom.CreatedBy;
            inputClassroom.UpdatedDate = GetRandomDateTime();

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.UpdatedDate),
                parameterValue: inputClassroom.UpdatedDate);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

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
        [MemberData(nameof(InvalidMinuteCases))]
        public async void ShouldThrowValidationExceptionOnCreateWhenCreatedDateIsNotRecentAndLogItAsync(
            int minutes)
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.UpdatedBy = inputClassroom.CreatedBy;
            inputClassroom.CreatedDate = dateTime.AddMinutes(minutes);
            inputClassroom.UpdatedDate = inputClassroom.CreatedDate;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.CreatedDate),
                parameterValue: inputClassroom.CreatedDate);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            // when
            ValueTask<Classroom> createClassroomTask =
                this.classroomService.CreateClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                createClassroomTask.AsTask());

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

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
