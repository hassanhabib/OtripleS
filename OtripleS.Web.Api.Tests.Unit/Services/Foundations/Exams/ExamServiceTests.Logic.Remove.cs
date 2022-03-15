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
        public async Task ShouldDeleteExamByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(dateTime);
            Guid inputExamId = randomExam.Id;
            Exam inputExam = randomExam;
            Exam storageExam = randomExam;
            Exam expectedExam = randomExam;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(inputExamId))
                    .ReturnsAsync(inputExam);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteExamAsync(inputExam))
                    .ReturnsAsync(storageExam);

            // when
            Exam actualExam =
                await this.examService.RemoveExamByIdAsync(inputExamId);

            // then
            actualExam.Should().BeEquivalentTo(expectedExam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(inputExamId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamAsync(inputExam),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
