// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using OtripleS.Web.Api.Models.StudentExams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomStudentExamId = default;
            Guid inputStudentExamId = randomStudentExamId;

            var invalidStudentExamInputException = new InvalidStudentExamInputException(
                parameterName: nameof(StudentExam.Id),
                parameterValue: inputStudentExamId);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(invalidStudentExamInputException);

            // when
            ValueTask<StudentExam> retrieveStudentExamByIdTask =
                this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                retrieveStudentExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveWhenStorageStudentExamIsNullAndLogItAsync()
        {
            // given
            Guid randomStudentExamId = Guid.NewGuid();
            Guid inputStudentExamId = randomStudentExamId;
            StudentExam invalidStorageStudentExam = null;
            var notFoundStudentExamException = new NotFoundStudentExamException(inputStudentExamId);

            var expectedStudentExamValidationException =
                new StudentExamValidationException(notFoundStudentExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ReturnsAsync(invalidStorageStudentExam);

            // when
            ValueTask<StudentExam> retrieveStudentExamByIdTask =
                this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            await Assert.ThrowsAsync<StudentExamValidationException>(() =>
                retrieveStudentExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentExamValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
