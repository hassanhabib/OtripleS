// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.StudentExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.StudentExamFees
{
    public partial class StudentExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldRemoveStudentExamFeeAsync()
        {
            // given
            var randomStudentId = Guid.NewGuid();
            var randomExamFeeId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid inputExamFeeId = randomExamFeeId;
            StudentExamFee randomStudentExamFee = CreateRandomStudentExamFee();
            randomStudentExamFee.StudentId = inputStudentId;
            randomStudentExamFee.ExamFeeId = inputExamFeeId;
            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = storageStudentExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentId, inputExamFeeId))
                        .ReturnsAsync(storageStudentExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee))
                    .ReturnsAsync(expectedStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.RemoveStudentExamFeeByIdAsync(
                    inputStudentId, inputExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentId, inputExamFeeId),
                        Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteStudentExamFeeAsync(storageStudentExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}