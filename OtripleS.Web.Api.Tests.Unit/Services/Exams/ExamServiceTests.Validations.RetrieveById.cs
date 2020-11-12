// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomExamId = default;
            Guid inputExamId = randomExamId;

            var invalidExamInputException = new InvalidExamInputException(
                parameterName: nameof(Exam.Id),
                parameterValue: inputExamId);

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> retrieveExamByIdTask =
                this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                retrieveExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async void ShouldThrowValidationExceptionOnRetrieveByIdWhenStorageExamIsNullAndLogItAsync()
        {
            // given
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            Exam invalidStorageExam = null;
            var notFoundExamException = new NotFoundExamException(inputExamId);

            var expectedExamValidationException =
                new ExamValidationException(notFoundExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ReturnsAsync(invalidStorageExam);

            // when
            ValueTask<Exam> retrieveExamByIdTask =
                this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                retrieveExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
