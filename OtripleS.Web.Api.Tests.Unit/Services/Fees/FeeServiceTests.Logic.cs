// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using OtripleS.Web.Api.Models.Fees;
using Xunit;
using Force.DeepCloner;

namespace OtripleS.Web.Api.Tests.Unit.Services.Fees
{
    public partial class FeeServiceTests
    {
        [Fact]
        public async Task ShouldAddFeeAsync()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            DateTimeOffset dateTime = randomDateTime;
            Fee randomFee = CreateRandomFee(randomDateTime);
            randomFee.UpdatedBy = randomFee.CreatedBy;
            Fee inputFee = randomFee;
            Fee storageFee = randomFee;
            Fee expectedFee = storageFee;

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertFeeAsync(inputFee))
                    .ReturnsAsync(storageFee);

            // when
            Fee actualFee =
                await this.feeService.AddFeeAsync(inputFee);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertFeeAsync(inputFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public void ShouldRetrieveAllFees()
        {
            // given
            DateTimeOffset randomDateTime = GetRandomDateTime();
            IQueryable<Fee> randomFees =
                CreateRandomFees(randomDateTime);

            IQueryable<Fee> storageFees =
                randomFees;

            IQueryable<Fee> expectedFees =
                storageFees;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectAllFees())
                    .Returns(storageFees);

            // when
            IQueryable<Fee> actualFees =
                this.feeService.RetrieveAllFees();

            // then
            actualFees.Should().BeEquivalentTo(expectedFees);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectAllFees(),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldRetrieveFeeByIdAsync()
        {
            // given
            Guid randomFeeId = Guid.NewGuid();
            Guid inputFeeId = randomFeeId;
            DateTimeOffset randomDateTime = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomDateTime);
            Fee storageFee = randomFee;
            Fee expectedFee = storageFee;

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(inputFeeId))
                    .ReturnsAsync(storageFee);

            // when
            Fee actualFee = 
                await this.feeService.RetrieveFeeByIdAsync(inputFeeId);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);

            this.storageBrokerMock.Verify(broker =>
                 broker.SelectFeeByIdAsync(inputFeeId),
                    Times.Once);

            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.dateTimeBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldModifyFeeAsync()
        {
            // given
            int randomNumber = GetRandomNumber();
            int randomDays = randomNumber;
            DateTimeOffset randomDate = GetRandomDateTime();
            DateTimeOffset randomInputDate = GetRandomDateTime();
            Fee randomFee = CreateRandomFee(randomInputDate);
            Fee inputFee = randomFee;
            Fee afterUpdateStorageFee = inputFee;
            Fee expectedFee = afterUpdateStorageFee;
            Fee beforeUpdateStorageFee = randomFee.DeepClone();
            inputFee.UpdatedDate = randomDate;
            Guid feeId = inputFee.Id;

            this.dateTimeBrokerMock.Setup(broker =>
               broker.GetCurrentDateTime())
                   .Returns(randomDate);

            this.storageBrokerMock.Setup(broker =>
                broker.SelectFeeByIdAsync(feeId))
                    .ReturnsAsync(beforeUpdateStorageFee);

            this.storageBrokerMock.Setup(broker =>
                broker.UpdateFeeAsync(inputFee))
                    .ReturnsAsync(afterUpdateStorageFee);

            // when
            Fee actualFee =
                await this.feeService.ModifyFeeAsync(inputFee);

            // then
            actualFee.Should().BeEquivalentTo(expectedFee);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.SelectFeeByIdAsync(feeId),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.UpdateFeeAsync(inputFee),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();            
        }
    }
}
