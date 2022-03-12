// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using FluentAssertions;
using Force.DeepCloner;
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
        public async Task ShouldModifyStudentExamAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            StudentExam randomStudentExam = CreateRandomStudentExam(randomInputDate);
            StudentExam inputStudentExam = randomStudentExam;
            StudentExam afterUpdateStorageStudentExam = inputStudentExam;
            StudentExam expectedStudentExam = afterUpdateStorageStudentExam;
            StudentExam beforeUpdateStorageStudentExam = randomStudentExam.DeepClone();
            inputStudentExam.UpdatedDate = randomDate;
            Guid studentId = inputStudentExam.StudentId;
            Guid guardianId = inputStudentExam.ExamId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExam.Id))
                    .ReturnsAsync(beforeUpdateStorageStudentExam);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentExamAsync(inputStudentExam))
                    .ReturnsAsync(afterUpdateStorageStudentExam);

            // when
            StudentExam actualStudentExam =
                await this.studentExamService.ModifyStudentExamAsync(inputStudentExam);

            // then
            actualStudentExam.Should().BeEquivalentTo(expectedStudentExam);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamByIdAsync(inputStudentExam.Id),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentExamAsync(inputStudentExam),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
