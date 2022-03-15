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
        public async Task ShouldRetrieveStudentExamFeeByIdAsync()
        {
            // given
            Guid randomStudentId = Guid.NewGuid();
            Guid inputStudentId = randomStudentId;
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            DateTimeOffset randomDateTime = GetRandomDateTime();

            StudentExamFee randomStudentExamFee =
                CreateRandomStudentExamFee(randomDateTime);

            StudentExamFee storageStudentExamFee = randomStudentExamFee;
            StudentExamFee expectedStudentExamFee = storageStudentExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentId, inputExamFeeId))
                        .ReturnsAsync(storageStudentExamFee);

            // when
            StudentExamFee actualStudentExamFee =
                await this.studentExamFeeService.RetrieveStudentExamFeeByIdsAsync(
                    inputStudentId, inputExamFeeId);

            // then
            actualStudentExamFee.Should().BeEquivalentTo(expectedStudentExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectStudentExamFeeByIdsAsync(
                    inputStudentId, inputExamFeeId),
                        Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
