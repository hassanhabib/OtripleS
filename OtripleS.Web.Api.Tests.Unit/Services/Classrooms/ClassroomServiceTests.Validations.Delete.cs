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

namespace OtripleS.Web.Api.Tests.Unit.Services.Classrooms
{
    public partial class ClassroomServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomClassroomId = default;
            Guid inputClassroomId = randomClassroomId;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.Id),
                parameterValue: inputClassroomId);

            var expectedClassroomValidationException = new ClassroomValidationException(invalidClassroomInputException);

            // when
            ValueTask<Classroom> actualClassroomTask =
                this.classroomService.DeleteClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() => actualClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteClassroomAsync(It.IsAny<Classroom>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteWhenStorageClassroomIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Classroom randomClassroom = CreateRandomClassroom(dates: randomDateTime);
            Guid inputClassroomId = randomClassroom.Id;
            Classroom inputClassroom = randomClassroom;
            Classroom nullStorageClassroom = null;

            var notFoundClassroomException = new NotFoundClassroomException(inputClassroomId);

            var expectedClassroomValidationException =
                new ClassroomValidationException(notFoundClassroomException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId))
                    .ReturnsAsync(nullStorageClassroom);

            // when
            ValueTask<Classroom> actualClassroomTask =
                this.classroomService.DeleteClassroomAsync(inputClassroomId);

            // then
            await Assert.ThrowsAsync<ClassroomValidationException>(() => actualClassroomTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectClassroomByIdAsync(inputClassroomId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteClassroomAsync(It.IsAny<Classroom>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
