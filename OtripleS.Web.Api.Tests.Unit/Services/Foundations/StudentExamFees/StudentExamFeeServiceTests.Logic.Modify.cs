// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Force.DeepCloner;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {

        [Fact]
        public async Task ShouldModifyStudentExamFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();

            StudentExamFee randomStudentExamFee =
                CreateRandomStudentExamFee(randomInputDate);

            StudentExamFee inputStudentExamFee = randomStudentExamFee;
            StudentExamFee afterUpdateStorageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = randomStudentExamFee;

            StudentExamFee beforeUpdateStorageStudentExamFee =
                randomStudentExamFee.DeepClone();

            inputStudentExamFee.UpdatedDate = randomDate;
            Guid studentId = inputStudentExamFee.StudentId;
            Guid guardianId = inputStudentExamFee.ExamFeeId;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentExamFee.StudentId,
                    inputStudentExamFee.ExamFeeId))
                        .ReturnsAsync(beforeUpdateStorageStudentExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateStudentExamFeeAsync(inputStudentExamFee))
                    .ReturnsAsync(afterUpdateStorageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.ModifyStudentExamFeeAsync(
                    inputStudentExamFee);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentExamFee.StudentId,
                    inputStudentExamFee.ExamFeeId),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateStudentExamFeeAsync(inputStudentExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
