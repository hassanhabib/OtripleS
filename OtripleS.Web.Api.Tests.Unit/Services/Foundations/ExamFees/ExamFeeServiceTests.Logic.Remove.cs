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
        public async Task ShouldDeleteExamFeeAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(dateTime);
            Guid inputExamFeeId = randomExamFee.Id;
            ExamFee inputExamFee = randomExamFee;
            ExamFee storageExamFee = inputExamFee;
            ExamFee expectedExamFee = storageExamFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId))
                    .ReturnsAsync(inputExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.DeleteExamFeeAsync(inputExamFee))
                    .ReturnsAsync(storageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.RemoveExamFeeByIdAsync(inputExamFeeId);

            //then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(inputExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.DeleteExamFeeAsync(inputExamFee),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }
    }
}
