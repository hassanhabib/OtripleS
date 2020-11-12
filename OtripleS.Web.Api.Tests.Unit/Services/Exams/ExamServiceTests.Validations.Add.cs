// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

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
        public async void ShouldThrowValidationExceptionOnAddWhenExamIsNullAndLogItAsync()
        {
            // given
            Exam randomExam = default;
            Exam nullExam = randomExam;
            var nullExamException = new NullExamException();

            var expectedExamValidationException =
                new ExamValidationException(nullExamException);

            // when
            ValueTask<Exam> createExamTask =
                this.examService.AddExamAsync(nullExam);

            // then
            await Assert.ThrowsAsync<ExamValidationException>(() =>
                createExamTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedExamValidationException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertExamAsync(It.IsAny<Exam>()),
                    Times.Never);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
