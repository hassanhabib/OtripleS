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
        public async Task ShouldThrowValidatonExceptionOnDeleteIfIdIsInvalidAndLogItAsync()
        {
            // given
            Guid randomExamId = Guid.Empty;
            Guid inputExamId = randomExamId;

            var invalidExamInputException = new InvalidExamException();

            invalidExamInputException.AddData(
                key: nameof(Exam.Id),
                values: "Id is required");

            var expectedExamValidationException =
                new ExamValidationException(invalidExamInputException);

            // when
            ValueTask<Exam> actualExamTask =
                this.examService.RemoveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() => actualExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(It.IsAny<Guid>()),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowValidatonExceptionOnDeleteIfStorageExamIsInvalidAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Guid inputExamId = randomExam.Id;
            Exam inputExam = randomExam;
            Exam nullStorageExam = null;

            var notFoundExamException = new NotFoundExamException(inputExamId);

            var expectedExamValidationException =
                new ExamValidationException(notFoundExamException);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ReturnsAsync(nullStorageExam);

            // when
            ValueTask<Exam> actualExamTask =
                this.examService.RemoveExamByIdAsync(inputExamId);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() => actualExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(
                    expectedExamValidationException))),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
