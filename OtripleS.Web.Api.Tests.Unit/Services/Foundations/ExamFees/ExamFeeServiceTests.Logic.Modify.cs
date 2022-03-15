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
using OtripleS.Web.Api.Models.ExamFees;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.Foundations.ExamFees
{
    public partial class ExamFeeServiceTests
    {
        [Fact]
        public async Task ShouldModifyExamFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            ExamFee randomExamFee = CreateRandomExamFee(randomInputDate);
            ExamFee inputExamFee = randomExamFee;
            ExamFee afterUpdateStorageExamFee = inputExamFee;
            ExamFee expectedExamFee = afterUpdateStorageExamFee;
            ExamFee beforeUpdateStorageExamFee = randomExamFee.DeepClone();
            inputExamFee.UpdatedDate = randomDate;
            Guid examExamFeeId = inputExamFee.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectExamFeeByIdAsync(examExamFeeId))
                    .ReturnsAsync(beforeUpdateStorageExamFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateExamFeeAsync(inputExamFee))
                    .ReturnsAsync(afterUpdateStorageExamFee);

            // when
            ExamFee actualExamFee =
                await this.examFeeService.ModifyExamFeeAsync(inputExamFee);

            // then
            actualExamFee.Should().BeEquivalentTo(expectedExamFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectExamFeeByIdAsync(examExamFeeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateExamFeeAsync(inputExamFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
        }
    }
}
