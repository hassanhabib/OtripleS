// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldAddStudentStudentExamAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            StudentExam randomStudentExam = CreateRandomStudentExam(dateTime);
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam storageStudentExam = randomStudentExam;
            StudentExam expectedStudentExam = storageStudentExam;

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentExamAsync(inputStudentExam))
                    .ReturnsAsync(storageStudentExam);

            // when
            StudentExam actualStudentExam =
                await this.studentEaxmService.AddStudentExamAsync(inputStudentExam);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
