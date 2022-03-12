// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.Exams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.Exams
{
    public partial class ExamServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveExamByIdAsync()
        {
            // given
            Guid randomExamId = Guid.NewGuid();
            Guid inputExamId = randomExamId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomDateTime);
            Exam storageExam = randomExam;
            Exam expectedExam = storageExam;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ReturnsAsync(storageExam);

            // when
            Exam actualExam =
                await this.examService.RetrieveExamByIdAsync(inputExamId);

            // then
            actualExam.Should().BeEquivalentTo(expectedExam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
