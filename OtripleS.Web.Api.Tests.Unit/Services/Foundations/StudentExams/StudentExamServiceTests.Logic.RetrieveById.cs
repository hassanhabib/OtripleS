// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExams
{
    public partial class StudentExamServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveStudentExamByIdAsync()
        {
            // given
            Guid randomStudentExamId = Guid.NewGuid();
            Guid inputStudentExamId = randomStudentExamId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(randomDateTime);
            StudentExam storageStudentExam = randomStudentExam;
            StudentExam expectedStudentExam = storageStudentExam;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId))
                    .ReturnsAsync(storageStudentExam);

            // when
            StudentExam actualStudentExam =
                await this.studentExamService.RetrieveStudentExamByIdAsync(inputStudentExamId);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExamId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
