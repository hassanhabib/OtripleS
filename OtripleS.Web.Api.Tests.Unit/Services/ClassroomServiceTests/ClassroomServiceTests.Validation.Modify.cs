// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Classrooms;
using OtripleS.Web.Api.Models.Classrooms.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.ClassroomServiceTests
{
	public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnModifyWhenClassroomIsNullAndLogItAsync()
        {
            //given
            Classroom invalidClassroom = null;
            var nullClassroomException = new NullClassroomException();

            var expectedClassroomValidationException =
                new ClassroomValidationException(nullClassroomException);

            //when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            //then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

		[Fact]
		public async Task ShouldThrowValidationExceptionOnModifyWhenClassroomIdIsInvalidAndLogItAsync()
		{
			//given
			Guid invalidClassroomId = Guid.Empty;
			DateTimeOffset dateTime = GetRandomDateTime();
			Classroom randomClassroom = CreateRandomClassroom(dateTime);
			Classroom invalidClassroom = randomClassroom;
			invalidClassroom.Id = invalidClassroomId;

			var invalidClassroomException = new InvalidClassroomInputException(
				parameterName: nameof(Classroom.Id),
				parameterValue: invalidClassroom.Id);

			var expectedClassroomValidationException =
				new ClassroomValidationException(invalidClassroomException);

			//when
			ValueTask<Classroom> modifyClassroomTask =
				this.classroomService.ModifyClassroomAsync(invalidClassroom);

			//then
			await Assert.ThrowsAsync<ClassroomValidationException>(() =>
				modifyClassroomTask.AsTask());

			this.loggingBrokerMock.Verify(broker =>
				broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
				Times.Once);

			this.loggingBrokerMock.VerifyNoOtherCalls();
			this.storageBrokerMock.VerifyNoOtherCalls();
			this.dateTimeBrokerMock.VerifyNoOtherCalls();
		}

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task ShouldThrowValidationExceptionOnModifyWhenClassroomNameIsInvalidAndLogItAsync(
                    string invalidClassroomName)
        {
            // given
            Classroom randomClassroom = CreateRandomClassroom(DateTime.Now);
            Classroom invalidClassroom = randomClassroom;
            invalidClassroom.Name = invalidClassroomName;

            var invalidClassroomException = new InvalidClassroomInputException(
               parameterName: nameof(Classroom.Name),
               parameterValue: invalidClassroom.Name);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomException);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(invalidClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
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
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;
            inputClassroom.CreatedBy = default;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.CreatedBy),
                parameterValue: inputClassroom.CreatedBy);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedByIsInvalidAndLogItAsync()
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
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenCreatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsInvalidAndLogItAsync()
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
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

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
        public async void ShouldThrowValidationExceptionOnModifyWhenUpdatedDateIsSameAsCreatedDateAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dateTime);
            Classroom inputClassroom = randomClassroom;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.UpdatedDate),
                parameterValue: inputClassroom.UpdatedDate);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> modifyClassroomTask =
                this.classroomService.ModifyClassroomAsync(inputClassroom);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                modifyClassroomTask.AsTask());

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
    }
}
