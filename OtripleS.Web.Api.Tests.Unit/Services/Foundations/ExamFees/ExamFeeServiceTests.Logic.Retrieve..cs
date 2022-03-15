// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.ExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldRetrieveExamFeeByIdAsync()
        {
            // given
            Guid randomExamFeeId = Guid.NewGuid();
            Guid inputExamFeeId = randomExamFeeId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(randomDateTime);
            ExamFee storageExamFee = randomExamFee;
            ExamFee expectedExamFee = storageExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.RetrieveExamFeeByIdAsync(inputExamFeeId);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Never);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
