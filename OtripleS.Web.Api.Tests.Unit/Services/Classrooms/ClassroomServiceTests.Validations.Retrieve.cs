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
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            //given
            Guid randomClassroomId = default;
            Guid inputClassroomId = randomClassroomId;

            var invalidClassroomInputException = new InvalidClassroomInputException(
                parameterName: nameof(Classroom.Id),
                parameterValue: inputClassroomId);

            var expectedClassroomValidationException =
                new ClassroomValidationException(invalidClassroomInputException);

            //when
            ValueTask<Classroom> retrieveClassroomByIdTask =
                this.classroomService.RetrieveClassroomById(inputClassroomId);

            //then
            await Assert.ThrowsAsync<ClassroomValidationException>(() => retrieveClassroomByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageClassroomIsNullAndLogItAsync()
        {
            //given
            Guid randomClassroomId = Guid.NewGuid();
            Guid inputClassroomId = randomClassroomId;
            Classroom invalidStorageClassroom = null;

            var notFoundClassroomException = new NotFoundClassroomException(inputClassroomId);

            var expectedClassroomValidationException =
                new ClassroomValidationException(notFoundClassroomException);

            this.storageBrokerMock.Setup(broker =>
                    broker.SelectClassroomByIdAsync(inputClassroomId))
                .ReturnsAsync(invalidStorageClassroom);

            //when
            ValueTask<Classroom> retrieveClassroomByIdTask =
                this.classroomService.RetrieveClassroomById(inputClassroomId);

            //then
            await Assert.ThrowsAsync<ClassroomValidationException>(() =>
                retrieveClassroomByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedClassroomValidationException))),
                Times.Once);

            this.dateTimeBrokerMock.Verify(broker => broker.GetCurrentDateTime(),
                Times.Never);

            this.storageBrokerMock.Verify(broker =>
                    broker.SelectClassroomByIdAsync(It.IsAny<Guid>()),
                Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}