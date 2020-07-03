using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Students;
using OtripleS.Web.Api.Models.Students.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentServiceTests
{
    public partial class StudentServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentId = default;
            Guid inputStudentId = randomStudentId;

            var invalidStudentInputException = new InvalidStudentInputException(
                parameterName: nameof(Student.Id),
                parameterValue: inputStudentId);

            var expectedStudentValidationException = 
                new StudentValidationException(invalidStudentInputException);

            // when
            ValueTask<Student> retrieveStudentByIdTask = 
                this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() => 
                retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageStudentIsNullAndLogItAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Student randomStudent = CreateRandomStudent();
            Student invalidStorageStudent = null;
            var nullStudentException = new NullStudentException();

            var expectedStudentValidationException =
                new StudentValidationException(nullStudentException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentByIdAsync(inputStudentId))
                    .ReturnsAsync(invalidStorageStudent);

            // when
            ValueTask<Student> retrieveStudentByIdTask =
                this.studentService.RetrieveStudentByIdAsync(inputStudentId);

            // then
            await Assert.ThrowsAsync<StudentValidationException>(() =>
                retrieveStudentByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentByIdAsync(inputStudentId),
                    Times.Once);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
