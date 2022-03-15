// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentStudentExamAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            randomStudentExam.UpdatedBy = randomStudentExam.CreatedBy;
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam storageStudentExam = randomStudentExam;
            StudentExam expectedStudentExam = storageStudentExam;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamAsync(inputStudentExam))
                    .ReturnsAsync(storageStudentExam);

            // when
            StudentExam actualStudentExam =
                await this.studentExamService.AddStudentExamAsync(inputStudentExam);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
