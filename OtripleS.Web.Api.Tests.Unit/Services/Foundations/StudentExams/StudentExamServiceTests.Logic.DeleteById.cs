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
using OtripleS.Web.Api.Models.StudentExams;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldDeleteStudentExamByIdAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            Guid inputStudentExamId = randomStudentExam.Id;
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam storageStudentExam = inputStudentExam;
            StudentExam expectedStudentExam = storageStudentExam;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ReturnsAsync(inputStudentExam);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentExamAsync(inputStudentExam))
                    .ReturnsAsync(storageStudentExam);

            // when
            StudentExam actualStudentExam =
                await this.studentExamService.RemoveStudentExamByIdAsync(inputStudentExamId);

            //then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
