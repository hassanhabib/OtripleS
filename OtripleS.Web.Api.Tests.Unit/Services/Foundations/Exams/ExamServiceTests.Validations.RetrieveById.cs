// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using OtripleS.Web.Api.Models.Exams.Exceptions;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldThrowValidationExceptionOnRetrieveByIdIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid invalidExamId = Guid.Empty;

            var invalidExamInputException = new InvalidExamException();

            invalidExamInputException.AddData(
                key: nameof(Exam.Id),
                values: "Id is required");

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> retrieveExamByIdTask =
                this.examService.RetrieveExamByIdAsync(invalidExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                retrieveExamByIdTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamValidationException))),
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
        public async void ShouldThrowValidationExceptionOnRetrieveByIdIfStorageExamIsNullAndLogItAsync()
        {
            // given
            Guid someExamId = Guid.NewGuid();
            Exam invalidStorageExam = null;

            var notFoundExamException =
                new NotFoundExamException(someExamId);

            var expectedExamValidationException =
                new ExamValidationException(notFoundExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(someExamId))
                    .ReturnsAsync(invalidStorageExam);

            // when
            ValueTask<Exam> retrieveExamByIdTask =
                this.examService.RetrieveExamByIdAsync(someExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                retrieveExamByIdTask.AsTask());

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(someExamId),
                    Times.Once);

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamValidationException))),
                        Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
