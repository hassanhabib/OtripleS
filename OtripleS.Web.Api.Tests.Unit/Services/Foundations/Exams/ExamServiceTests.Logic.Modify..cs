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
        public async Task ShouldModifyExamAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Exam randomExam = CreateRandomExam(randomInputDate);
            Exam inputExam = randomExam;
            Exam afterUpdateStorageExam = inputExam;
            Exam expectedExam = afterUpdateStorageExam;
            Exam beforeUpdateStorageExam = randomExam.DeepClone();
            inputExam.UpdatedDate = randomDate;
            Guid examId = inputExam.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamByIdAsync(examId))
                    .ReturnsAsync(beforeUpdateStorageExam);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateExamAsync(inputExam))
                    .ReturnsAsync(afterUpdateStorageExam);

            // when
            Exam actualExam =
                await this.examService.ModifyExamAsync(inputExam);

            // then
            actualExam.Should().BeEquivalentTo(expectedExam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamByIdAsync(examId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamAsync(inputExam),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
